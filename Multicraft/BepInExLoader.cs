using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using PugTilemap;
using UnhollowerRuntimeLib;

namespace Multicraft
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class BepInExLoader : BasePlugin
    {
        // Config Settings
        private static ConfigEntry<bool> _modEnabled;

        // Logger
        public static ManualLogSource log;

        public BepInExLoader()
        {
            log = Log;
        }

        public override void Load()
        {
            _modEnabled = Config.Bind("General", "Enabled", true, "Enable this mod");
            Config.Save();

            if (_modEnabled.Value)
            {
                try
                {
                    ClassInjector.RegisterTypeInIl2Cpp<Bootstrapper>();
                    ClassInjector.RegisterTypeInIl2Cpp<CraftComponent>();
                }
                catch
                {
                    log.LogError("Il2Cpp type failed to register!");
                }

                try
                {
                    var harmony = new Harmony(PluginInfo.PLUGIN_GUID);

                    var originalUpdate = AccessTools.Method(typeof(RecipeSlotUI), "OnLeftClicked");
                    var postUpdate = AccessTools.Method(typeof(Bootstrapper), "OnLeftClicked");
                    harmony.Patch(originalUpdate, postfix: new HarmonyMethod(postUpdate));
                }
                catch
                {
                    log.LogError("Failed to patch event");
                }

                Bootstrapper.Create();
            }
        }
    }
}