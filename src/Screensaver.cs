using System;
using System.Collections;
using System.Reflection;
using Modding;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Screensaver;
public class Screensaver : Mod
{
    new public string GetName () => "Screensaver";
    public override string GetVersion () => "1.0.0.0";

    private GameObject SSObj;
    private static Screensaver _instance;

    public static Screensaver Instance {
        get { return _instance; }
    }

    public override void Initialize ()
    {
        _instance = this;
        SSObj = new GameObject();
        SSObj.AddComponent<ScreensaverBehaviour>();
        Object.DontDestroyOnLoad(SSObj);
    }
}
