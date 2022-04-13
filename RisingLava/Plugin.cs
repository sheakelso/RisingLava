using System;
using System.Reflection;
using System.IO;
using System.Collections;
using BepInEx;
using UnityEngine;
using Utilla;
using ComputerInterface;
using ComputerInterface.Interfaces;
using Photon.Pun;
using GorillaNetworking;
using GorillaLocomotion;
using HarmonyLib;

namespace RisingLava
{ 
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [ModdedGamemode("RISINGLAVA3", "RISING LAVA", Utilla.Models.BaseGamemode.Infection)]
    public class Plugin : BaseUnityPlugin
    {
        public GameObject lavaPrefab;
        public GameObject lavaVisionPrefab;
        GameObject lavaVision;
        GameObject lavaVision2;
        bool init = false;
        bool vision = false;
        GameObject catcher;

        void Awake()
		{
            HarmonyPatches.ApplyHarmonyPatches();
            print("RISING LAVA AWAKE");
            StartCoroutine(LoadBundles());
        }

        void Update()
        {
            if (!catcher)
            {
                new GameObject("BannedGUIDChecker").AddComponent<BannedModsChecker>();
                catcher = new GameObject("EventCatcher");
                catcher.AddComponent<KickEvent>();
            }
            if(lavaPrefab != null && !init)
            {
                GameObject lava = Instantiate<GameObject>(lavaPrefab);
                MeshRenderer[] children = lava.transform.GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer renderer in children)
                {
                    renderer.material.mainTextureScale = new Vector2(40, 40);
                }
                lava.AddComponent<LavaManager>();
                print("LAVA INSTATNIATED");
                init = true;
            }

            if (PhotonNetwork.InRoom)
            {
                string queue = GorillaComputer.instance.currentGameMode;
                print(queue);
                if (queue == "MODDED_RISINGLAVA3INFECTION" && PhotonNetwork.CurrentRoom.PlayerCount > 1)
                {
                    LavaManager.instance.gameObject.SetActive(true);
                    FieldInfo info = AccessTools.Field(typeof(GorillaTagManager), "infectedModeThreshold");
                    info.SetValue(GorillaTagManager.instance, 2);
                    //enable lavavision if head is below lava
                    if(Player.Instance.headCollider.transform.position.y < LavaManager.instance.transform.position.y)
                    {
                        lavaVision.SetActive(true);
                        lavaVision2.SetActive(true);
                    }
                    else
                    {
                        lavaVision.SetActive(false);
                        lavaVision2.SetActive(false);
                    }
                }
                else
                {
                    LavaManager.instance.ResetLava();
                    LavaManager.instance.gameObject.SetActive(false);
                    lavaVision.SetActive(false);
                    lavaVision2.SetActive(false);
                }
            }
            else
            {
                LavaManager.instance.ResetLava();
                LavaManager.instance.gameObject.SetActive(false);
                lavaVision.SetActive(false);
                lavaVision2.SetActive(false);

            }
        }
        IEnumerator LoadBundles()
        {
            Stream lavaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RisingLava.Resources.LavaAsset");
            AssetBundleCreateRequest lavaRequest = AssetBundle.LoadFromStreamAsync(lavaStream);
            yield return lavaRequest;

            AssetBundle lavaBundle = lavaRequest.assetBundle;
            AssetBundleRequest lavaAssetReq = lavaBundle.LoadAssetAsync<GameObject>("_Hat");
            yield return lavaAssetReq;

            print("LAVA LOADED");
            lavaPrefab = (GameObject)lavaAssetReq.asset;

            Stream lavaStream1 = Assembly.GetExecutingAssembly().GetManifestResourceStream("RisingLava.Resources.LavaVision");
            AssetBundleCreateRequest lavaRequest1 = AssetBundle.LoadFromStreamAsync(lavaStream1);
            yield return lavaRequest1;

            AssetBundle lavaBundle1 = lavaRequest1.assetBundle;
            AssetBundleRequest lavaAssetReq1 = lavaBundle1.LoadAssetAsync<GameObject>("_Hat");
            yield return lavaAssetReq1;

            print("LAVA LOADED");
            lavaVisionPrefab = (GameObject)lavaAssetReq1.asset;

            lavaVision = Instantiate<GameObject>(lavaVisionPrefab);
            lavaVision.transform.SetParent(GorillaLocomotion.Player.Instance.headCollider.transform);
            lavaVision.transform.localPosition = new Vector3(0, 0, 0.3f);
            lavaVision.transform.localEulerAngles = new Vector3(0, 90, 0);
            lavaVision.layer = 50;
            lavaVision.SetActive(false);

            lavaVision2 = Instantiate<GameObject>(lavaVisionPrefab);
            GameObject.Find("Shoulder Camera").SetActive(false);
            lavaVision2.transform.localPosition = new Vector3(0, 0, 0.3f);
            lavaVision2.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
    }
}
