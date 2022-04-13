using Photon.Pun;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using GorillaLocomotion;
using GorillaNetworking;
using System.Reflection;

namespace RisingLava.Patches
{
	[HarmonyPatch(typeof(GorillaNetworkJoinTrigger))]
	[HarmonyPatch("OnBoxTriggered", MethodType.Normal)]
	internal class MapCheckPatch
	{
		private static void Postfix(GorillaNetworkJoinTrigger __instance)
		{
			LavaManager.instance.SetMap(__instance.gameModeName);
			return;
		}
    }
}
