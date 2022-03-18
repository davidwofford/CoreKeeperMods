using System;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerBaseLib;
using UnityEngine.EventSystems;

namespace Multicraft;

public class CraftComponent : MonoBehaviour
{
    public static GameObject GameObj = null;
    public static CraftComponent Instance;
    public static bool ShouldFireEvent = true;

    public CraftComponent(IntPtr ptr) : base(ptr)
    {
        Instance = this;
    }

    internal static GameObject Create()
    {
        GameObj = new GameObject("ClockObject");
        DontDestroyOnLoad(GameObj);

        var component = new CraftComponent(GameObj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<CraftComponent>()).Pointer);

        return GameObj;
    }

    public void OnLeftClicked(RecipeSlotUI __instance)
    {
        if (!ShouldFireEvent)
        {
            return;
        }

        BepInExLoader.log.LogInfo("OnLeftClicked Fired");

        ShouldFireEvent = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            BepInExLoader.log.LogInfo("Crafting 10 items");
            for (var i = 0; i < 9; i++)
            {
                __instance.OnLeftClicked(false, false);
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            BepInExLoader.log.LogInfo("Crafting as 25 items");
            for (var i = 0; i < 24; i++)
            {
                __instance.OnLeftClicked(false, false);
            }
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            BepInExLoader.log.LogInfo("Crafting as 100 items");
            for (var i = 0; i < 99; i++)
            {
                __instance.OnLeftClicked(false, false);
            }
        }

        ShouldFireEvent = true;
    }
}