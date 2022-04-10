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
