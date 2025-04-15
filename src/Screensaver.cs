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
    public int count = 1;
    public float screenPercentage = 0.25f;
    public float speed = 0.15f;
}

public class Screensaver : Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
{
    public static GlobalSettings settings {get; set;} = new GlobalSettings();

    public void OnLoadGlobal(GlobalSettings s) {
        settings = s;
        foreach (var behaviour in behaviours)
        {
            behaviour.ToggleScreensaver(settings.enabled);
            behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
        }
    }
    public GlobalSettings OnSaveGlobal() => settings;

    new public string GetName () => "Screensaver";
    public override string GetVersion () => "1.1.0.1";

    private GameObject SSObj;
    private List<ScreensaverBehaviour> behaviours = new();
    private static Screensaver _instance;

    public static Screensaver Instance {
        get { return _instance; }
    }

    public override void Initialize ()
    {
        _instance = this;
        SSObj = new GameObject();
        for (int i = 0; i < settings.count; i++)
        {
            var behaviour = SSObj.AddComponent<ScreensaverBehaviour>();
            behaviour.ToggleScreensaver(settings.enabled);
            behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
            behaviours.Add(behaviour);
        }
        Object.DontDestroyOnLoad(SSObj);
    }

    private void UpdateToggle()
    {
        foreach (var behaviour in behaviours)
        {
            behaviour.ToggleScreensaver(settings.enabled);
        }
    }

    private void UpdateColorOnBounce()
    {
        foreach (var behaviour in behaviours)
        {
            behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
        }
    }

    private void UpdateCount()
    {
        if (behaviours.Count > settings.count)
        {
            for (int i = behaviours.Count - 1; i >= settings.count; i--)
            {
                var behaviour = behaviours[i];
                behaviours.RemoveAt(i);
                Object.Destroy(behaviour);
            }
        }
        else if (behaviours.Count < settings.count)
        {
            for (int i = behaviours.Count; i < settings.count; i++)
            {
                var behaviour = SSObj.AddComponent<ScreensaverBehaviour>();
                behaviour.ToggleScreensaver(settings.enabled);
                behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
                behaviours.Add(behaviour);
            }
        }
    }

    private Menu menuRef;

    private Menu PrepareMenu()
    {
        return new Menu("Screensaver", new Element[]{
            Blueprints.HorizontalBoolOption("Screensaver",
                "Toggle if the screensaver is active",
                (option) => { settings.enabled = option; UpdateToggle(); },
                () => settings.enabled
            ),
            Blueprints.HorizontalBoolOption("Change color on bounce", 
                "Toggle if the color of the screensaver should change on bounce",
                (option) => { settings.changeColorOnBounce = option; UpdateColorOnBounce(); },
                () => settings.changeColorOnBounce
            ),
            new FixedCustomSlider("Screensaver Count",
                (option) => { settings.count = (int)option; UpdateCount(); },
                () => settings.count,
                1f, 25.0f, true
            ),
            new PercentSlider("Screen Percentage",
                (option) => settings.screenPercentage = option,
                () => settings.screenPercentage,
                0.01f, 0.5f, false
            ),
            new FixedCustomSlider("Screensaver Speed",
                (option) => settings.speed = option,
                () => settings.speed,
                0.1f, 5.0f, false
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
