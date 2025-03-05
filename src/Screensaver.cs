using System;
using System.Collections.Generic;
using System.Reflection;
using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Satchel.BetterMenus;

namespace Screensaver;

public class GlobalSettings
{
    public bool enabled = true;
    public bool changeColorOnBounce = true;
    public float screenPercentage = 0.25f;
}

public class Screensaver : Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
{
    public static GlobalSettings settings {get; set;} = new GlobalSettings();

    public void OnLoadGlobal(GlobalSettings s) {
        settings = s;
        if (behaviour != null)
        {
            behaviour.ToggleScreensaver(settings.enabled);
            behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
        }
    }
    public GlobalSettings OnSaveGlobal() => settings;

    new public string GetName () => "Screensaver";
    public override string GetVersion () => "1.1.0.1";

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
        behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
    }

    private Menu menuRef;

    private Menu PrepareMenu()
    {
        return new Menu("Screensaver", new Element[]{
            new  HorizontalOption("Screensaver", 
                "Toggle if the screensaver is active",
                new string[] {"On", "Off"},
                (option) => { settings.enabled = option == 0; behaviour.ToggleScreensaver(settings.enabled); },
                () => settings.enabled ? 0 : 1
            ),
            new  HorizontalOption("Change color on bounce", 
                "Toggle if the color of the screensaver should change on bounce",
                new string[] {"On", "Off"},
                (option) => { settings.changeColorOnBounce = option == 0; behaviour.ToggleColorOnBounce(settings.changeColorOnBounce); },
                () => settings.changeColorOnBounce ? 0 : 1
            ),
            new PercentSlider("Screen Percentage",
                (option) => settings.screenPercentage = option,
                () => settings.screenPercentage,
                0.01f, 0.5f, false
            ),
        });
    }

    public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
    {
        menuRef ??= PrepareMenu();

        return menuRef.GetMenuScreen(modListMenu);
    }

    public bool ToggleButtonInsideMenu => false;
}
