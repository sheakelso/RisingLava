using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;
using GorillaNetworking;

namespace RisingLava
{
    class LavaManager : MonoBehaviour
    {
        public static LavaManager instance;

        public static float height;

        Vector3 startPos = new Vector3(-50, -3, -50);
        Vector3 endPos = new Vector3(-50, 35, -50);

        Dictionary<string, Vector3[]> mapCoords = new Dictionary<string, Vector3[]>
        {
            { "forest", new Vector3[] { new Vector3(-50, -3, -50), new Vector3(-50, 35, -50) }},
            { "cave", new Vector3[] { new Vector3(-50, -21, -50), new Vector3(-50, -2, -50) }},
            { "canyon", new Vector3[] { new Vector3(-100, -6, -130), new Vector3(-100, 20, -130) }},
            { "mountain", new Vector3[] { new Vector3(30, -9, -80), new Vector3(30, 20, -80) }},
        };

        string currentMap;

        object gameEnd = null;

        void Start()
        {
            instance = this;
            transform.position = startPos;
            //gameObject.SetActive(false);
        }

        void Update()
        {
            object obj;
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("timeInfectedGameEnded", out obj);
            if(obj != gameEnd)
            {
                ResetLava();
                gameEnd = obj;
            }
            transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 0.005f);
            height = transform.position.y;
        }

        public void ResetLava()
        {
            transform.position = startPos;
        }

        public void SetMap(string map)
        {
            currentMap = map;
            startPos = mapCoords[map][0];
            endPos = mapCoords[map][1];
        }

        public static void TagLocalPlayer()
        {
            if (PhotonNetwork.InRoom)
            { 
                PhotonView.Get(GorillaTagManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
               {
                   PhotonNetwork.LocalPlayer
                });
            }
        }
    }
}
