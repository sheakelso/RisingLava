using HarmonyLib;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("PlayTagSound", MethodType.Normal)]
    internal class RoundEndPatch
    {
        internal static void Postfix(int soundIndex, float soundVolume)
        {
            if(soundIndex == 2)
            {
                LavaManager.instance.ResetLava();
            }
        }
    }
}
