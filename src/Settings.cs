using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Screensaver;

public class GlobalSettings
{
    public bool enabled = true;
    public bool changeColorOnBounce = true;

    [Range(1, 25)]
    public int count = 1;

    [Range(0.01f, 0.5f)]
    public float screenPercentage = 0.25f;

    [Range(0.1f, 2f)]
    public float speed = 0.15f;

    public string saverName = "";
}

#pragma warning disable CS0649
internal class SaverSettings
{
    [DefaultValue(false)]
    public bool IsAnimated;
    [DefaultValue(0.0f)]
    public float FPS;
    [DefaultValue(0.0f)]
    public float FrameTime;

    [DefaultValue(null)]
    public List<string> FileNames;
}
#pragma warning restore CS0649
