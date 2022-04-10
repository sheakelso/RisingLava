using Photon.Pun;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using GorillaNetworking;

namespace RisingLava.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("LateUpdate", MethodType.Normal)]
	internal class MaterialPatch
	{
		static bool prev;
		static int index = 0;
		private static void Postfix(VRRig __instance)
        {
			bool btn;
            InputDevice r = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
			r.TryGetFeatureValue(CommonUsages.secondaryButton, out btn);
            if (btn)
            {
				if (index == 0) index = 1;
				else index = 0;
				//__instance.mainSkin.material = __instance.materialsToChangeTo[index];
            }
			prev = btn;
		}
    }
}
