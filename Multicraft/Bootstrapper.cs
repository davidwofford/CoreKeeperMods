using HarmonyLib;
using UnityEngine;
using System;

namespace Multicraft;

public class Bootstrapper : MonoBehaviour
{
    private static GameObject craftComponent = null;

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
    public static void OnLeftClicked(RecipeSlotUI __instance)
    {
        if (craftComponent == null)
        {
            try
            {
                craftComponent = CraftComponent.Create();
            }
            catch(Exception e)
            {
                BepInExLoader.log.LogError("Error bootstrapping clock: " + e.Message);
            }
        }

        CraftComponent.Instance.OnLeftClicked(__instance);
    }
}