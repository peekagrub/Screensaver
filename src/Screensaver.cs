using System;
using System.Collections.Generic;
using System.Reflection;
using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Screensaver;

public class GlobalSettings
{
    public bool enabled = true;
    public float screenPercentage = 0.25f;
}

public class Screensaver : Mod, IGlobalSettings<GlobalSettings>, IMenuMod
{
    public static GlobalSettings settings {get; set;} = new GlobalSettings();

    public void OnLoadGlobal(GlobalSettings s) {
         settings = s;
         if (behaviour != null)
         {
             behaviour.ToggleScreensaver(settings.enabled);
         }
    }
    public GlobalSettings OnSaveGlobal() => settings;

    new public string GetName () => "Screensaver";
    public override string GetVersion () => "1.1.0.0";

    private ScreensaverBehaviour behaviour;
    private static Screensaver _instance;

    public static Screensaver Instance {
        get { return _instance; }
    }

    public override void Initialize ()
    {
        _instance = this;
        GameObject SSObj = new GameObject();
        behaviour = SSObj.AddComponent<ScreensaverBehaviour>();
        Object.DontDestroyOnLoad(SSObj);
        behaviour.ToggleScreensaver(settings.enabled);
    }

    public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
    {
        return new List<IMenuMod.MenuEntry>
        {
            new IMenuMod.MenuEntry
            {
                Name = "Screensaver",
                Description = "Toggle if the screensaver is active",
                Values = new string []{
                    "On",
                    "Off"
                },
                Saver = opt => { settings.enabled = opt == 0; behaviour.ToggleScreensaver(settings.enabled); },
                Loader = () => settings.enabled ? 0 : 1
            },
            new  IMenuMod.MenuEntry
            {
                Name = "Screen Percentage",
                Description = "The percentage of the screen that the screensaver covers",
                Values = new string[] {
                    "5%",
                    "10%",
                    "15%",
                    "20%",
                    "25%",
                    "30%",
                    "35%",
                    "40%",
                    "45%",
                    "50%",
                },
                Saver = opt => { settings.screenPercentage = 0.05f * (opt + 1); },
                Loader = () => (int)(settings.screenPercentage / 0.05f) - 1
            },
        };
    }

    public bool ToggleButtonInsideMenu => false;
}
