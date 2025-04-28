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
    public override string GetVersion () => "1.1.1.0";

    private GameObject SSObj;
    private List<ScreensaverBehaviour> behaviours = new();
    private static Screensaver _instance;

    public static Screensaver Instance { get { return _instance; } }
    public bool ToggleButtonInsideMenu { get; } = false;

    public void OnLoadGlobal(GlobalSettings s) 
    {
        settings = s;
        foreach (var behaviour in behaviours)
        {
            behaviour.ToggleScreensaver(settings.enabled);
            behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
        }
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
        for (int i = 0; i < settings.count; i++)
        {
            var behaviour = SSObj.AddComponent<ScreensaverBehaviour>();
            behaviour.ToggleScreensaver(settings.enabled);
            behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
            behaviours.Add(behaviour);
        }
        Object.DontDestroyOnLoad(SSObj);
    }

    internal void UpdateToggle()
    {
        foreach (var behaviour in behaviours)
        {
            behaviour.ToggleScreensaver(settings.enabled);
        }
    }

    internal void UpdateColorOnBounce()
    {
        foreach (var behaviour in behaviours)
        {
            behaviour.ToggleColorOnBounce(settings.changeColorOnBounce);
        }
    }

    internal void UpdateCount()
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

    public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
    {
        return ModMenu.GetMenu(modListMenu, toggleDelegates);
    }
}
