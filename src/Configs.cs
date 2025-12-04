using BepInEx.Configuration;
using LLBML.Utils;

namespace TourneyInfo;

public class Configs
{
    private static ConfigFile config;
    
    internal static ConfigEntry<string> TourneyName { get; private set; }
    internal static ConfigEntry<string> TourneyDescriptionLine1 { get; private set; }
    internal static ConfigEntry<string> TourneyDescriptionLine2 { get; private set; }
    
    internal static ConfigEntry<string> ModpackVersion { get; private set; }
    
    internal static void Init()
    {
        config = Plugin.Instance.Config;
        
        config.Bind("Headers", "mm_header_tourney", "Tourney Info", new ConfigDescription("", null, "modmenu_header"));

        TourneyName = config.Bind<string>("Tourney Info", "TourneyName", "LAN Tourney");
        TourneyDescriptionLine1 = config.Bind<string>("Tourney Info", "TourneyDescriptionLine1", "LLB Stadium");
        TourneyDescriptionLine2 = config.Bind<string>("Tourney Info", "TourneyDescriptionLine2", "discord.gg/llbstadium");
        
        config.Bind("gap", "mm_header_gap", 50, new ConfigDescription("", null, "modmenu_gap"));
        config.Bind("Headers", "mm_header_modpack", "Modpack Info", new ConfigDescription("", null, "modmenu_header"));

        ModpackVersion = config.Bind<string>("Modpack Info", "ModpackVersion", "");
        
        ModDependenciesUtils.RegisterToModMenu(Plugin.Instance.Info, [
            "Allows you to display a custom description, name, and logo for your tournament",
            "The logo file must be named 'logo.png' and should be placed in the TourneyInfo Modding Folder"
        ]);
    }
}