using HarmonyLib;
using LLHandlers;
using TMPro;

namespace TourneyInfo;

internal static class HarmonyPatches
{
    internal static void PatchAll()
    {
        Harmony harmony = new Harmony(Plugin.GUID);
        
        harmony.PatchAll(typeof(PlayerInfoPatch));
        Plugin.LogGlobal.LogInfo("Player info patches applied");
    }

    private static class PlayerInfoPatch
    {
        [HarmonyPatch(typeof(ScreenMenu), nameof(ScreenMenu.OnOpen))]
        [HarmonyPostfix]
        private static void OnOpen_Postfix(ScreenMenu __instance)
        {
            Plugin.Instance.Config.SettingChanged += (sender, args) => __instance.UpdatePlayerInfo();
            __instance.lbLevel.enableWordWrapping = false;

            __instance.lbVersion.overflowMode = TextOverflowModes.Overflow;
            __instance.lbVersion.enableWordWrapping = false;
        }
        
        [HarmonyPatch(typeof(ScreenMenu), nameof(ScreenMenu.UpdatePlayerInfo))]
        [HarmonyPostfix]
        private static void UpdatePlayerInfo_Postfix(ScreenMenu __instance)
        {
            __instance.imAvatar.texture = JPLELOFJOOH.BNFIDCAPPDK("avatarImageDefault").texture;
            __instance.btAccount.SetText(Configs.TourneyName.Value, -1, true);
            
            TextHandler.SetText(__instance.lbLevel,$"{Configs.TourneyDescriptionLine1.Value}\n{Configs.TourneyDescriptionLine2.Value}");

            if (string.IsNullOrEmpty(Configs.ModpackVersion.Value))
            {
                TextHandler.SetText(__instance.lbVersion, $"V{JPLELOFJOOH.MDGMBEMFDJK}");
            }
            else
            {
                TextHandler.SetText(__instance.lbVersion, $"Modpack v{Configs.ModpackVersion.Value}");
            }
        }
    }
}