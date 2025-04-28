using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Screensaver;

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
