using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Modding;
using Modding.Menu;
using Satchel.BetterMenus;

namespace Screensaver.Menus;

public class PercentSlider : FixedCustomSlider
{
    public PercentSlider(string name, Action<float> storeValue, Func<float> loadValue, float minValue, float maxValue, bool wholeNumbers = false, string Id = "__UseName")
        : base(name, storeValue, loadValue, minValue, maxValue, wholeNumbers, Id)
    {
    }

    protected override void UpdateValueLabel()
    {
        valueLabel.text = $"{value * 100:0}%\n";
    }
}
