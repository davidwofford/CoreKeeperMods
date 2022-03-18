using System;
using HarmonyLib;
using UnityEngine;

namespace Clock
{
    public class Bootstrapper : MonoBehaviour
    {
        private static GameObject clock = null;

        internal static GameObject Create()
        {
            var obj = new GameObject("ClockBootstrapperObject");
            DontDestroyOnLoad(obj);
            var component = new Bootstrapper(obj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<Bootstrapper>()).Pointer);
            return obj;
        }

        public Bootstrapper(IntPtr intPtr) : base(intPtr) {}

        public void Awake()
        {
            // Do nothing but have a method to prevent errors
        }

        [HarmonyPostfix]
        public static void Update()
        {
            if (clock == null)
            {
                try
                {
                    clock = ClockComponent.Create();
                }
                catch(Exception e)
                {
                    BepInExLoader.log.LogError("Error bootstrapping clock: " + e.Message);
                }
            }
        }
    }
}