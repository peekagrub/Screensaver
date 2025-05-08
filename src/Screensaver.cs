using System;
using System.Collections.Generic;
using System.Reflection;
using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Screensaver.Menus;

namespace Screensaver;

public class Screensaver : Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
{
    internal static GlobalSettings settings {get; set;} = new GlobalSettings();

    new public string GetName () => "Screensaver";
    public override string GetVersion () => "1.2.0.0";

    private GameObject SSObj;
    private ScreensaverBehaviour behaviour;
    private static Screensaver _instance;

    public static Screensaver Instance { get { return _instance; } }
    public bool ToggleButtonInsideMenu { get; } = false;

    public void OnLoadGlobal(GlobalSettings s) 
    {
        settings = s;
        behaviour?.ToggleScreensaver(settings.enabled);
        behaviour?.ToggleColorOnBounce(settings.changeColorOnBounce);
    }
    public GlobalSettings OnSaveGlobal()
    {
        settings.saverName = ScreensaverManager.Instance.CurrentScreensaver.Name;

        return settings;
    }

    public override void Initialize ()
    {
        _instance = this;
        ScreensaverManager.Instance.SetScreensaverByName(settings.saverName);
        SSObj = new GameObject();

        behaviour = SSObj.AddComponent<ScreensaverBehaviour>();
        behaviour.SetCount(settings.count);
        behaviour.ToggleScreensaver(settings.enabled);
        behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);

        Object.DontDestroyOnLoad(SSObj);
    }

    internal void UpdateToggle()
    {
        behaviour.ToggleScreensaver(settings.enabled);
    }

    internal void UpdateColorOnBounce()
    {
        behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
    }

    internal void UpdateCount()
    {
        behaviour.SetCount(settings.count);
    }

    public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
    {
        return ModMenu.GetMenu(modListMenu, toggleDelegates);
    }
}
