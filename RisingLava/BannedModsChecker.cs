using System.Collections;
using UnityEngine.Networking;
using BepInEx.Bootstrap;
using UnityEngine;

namespace RisingLava
{
    public class BannedModsChecker : MonoBehaviour
    {
        void Awake()
        {
            StartCoroutine(GetBannedGUIDs());
        }

        IEnumerator GetBannedGUIDs()
        {
            UnityWebRequest bannedGUIDsReq = UnityWebRequest.Get("https://raw.githubusercontent.com/ChillGunner/TestRepo/main/testfile");
            yield return bannedGUIDsReq.SendWebRequest();

            if (bannedGUIDsReq.responseCode >= 200 && bannedGUIDsReq.responseCode < 300)
            {
                string[] bannedGUIDs = Base64Decode(bannedGUIDsReq.downloadHandler.text).Split('\n');
                CheckGUIDs(bannedGUIDs);
            }
            else
            {
                Application.Quit();
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        void CheckGUIDs(string[] bannedGUIDs)
        {
            foreach (string bannedGUID in bannedGUIDs)
            {
                if (Chainloader.PluginInfos.ContainsKey(bannedGUID))
                {
                    Application.Quit();
                }
            }
        }
    }
}