using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using PugTilemap;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Clock
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class BepInExLoader : BasePlugin
    {
        // Config Settings
        private static ConfigEntry<bool> _modEnabled;
        public static ConfigEntry<bool> DebugMode;
        public static ConfigEntry<bool> TwelveHourFormat;

        // Logger
        public static ManualLogSource log;

        public BepInExLoader()
        {
            log = Log;
        }

        public override void Load()
        {
            _modEnabled = Config.Bind("General", "Enabled", true, "Enable this mod");
            DebugMode = Config.Bind("General", "Debug", true, "Enable debug mode for this mod");
            TwelveHourFormat = Config.Bind("General", "12_Hour_Format", true,
                "Whether time should be in 12 or 24h format");
            Config.Save();

            if (_modEnabled.Value)
            {
                try
                {
                    ClassInjector.RegisterTypeInIl2Cpp<Bootstrapper>();
                    ClassInjector.RegisterTypeInIl2Cpp<ClockComponent>();
                    //ClassInjector.RegisterTypeInIl2Cpp<TextManager>();
                }
                catch
                {
                    log.LogError("Il2Cpp type failed to register!");
                }

                try
                {
                    var harmony = new Harmony(PluginInfo.PLUGIN_GUID);

                    var originalUpdate = AccessTools.Method(typeof(SinglePugMap), "Update");
                    var postUpdate = AccessTools.Method(typeof(Bootstrapper), "Update");
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