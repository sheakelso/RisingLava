using System;
using System.Collections.Generic;
using System.Text;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using ComputerInterface;
using UnityEngine;

internal class KickEvent : MonoBehaviour, IOnEventCallback
{
    public void OnEvent(EventData eventData)
    {
        if (eventData.Code == 181)
        {
            if ((string)eventData.CustomData == PhotonNetwork.LocalPlayer.UserId)
            {
                BaseGameInterface.Disconnect();
            }
        }
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
