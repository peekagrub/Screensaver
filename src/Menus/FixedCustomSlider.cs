using System;
using Modding;
using Modding.Menu;
using Satchel.BetterMenus;

namespace Screensaver.Menus;

public class FixedCustomSlider : CustomSlider
{
#pragma warning disable CS0618
    public FixedCustomSlider(string name, Action<float> storeValue, Func<float> loadValue, string Id = "__UseName") 
        : base(name, storeValue, loadValue, Id)
    {
        if (!Name.EndsWith("\n"))
        {
            Name += "\n";
        }
    }
#pragma warning restore CS0618

    public FixedCustomSlider(string name, Action<float> storeValue, Func<float> loadValue, float minValue, float maxValue, bool wholeNumbers = false, string Id = "__UseName")
        : base(name, storeValue, loadValue, minValue, maxValue, wholeNumbers, Id)
    {
        if (!Name.EndsWith("\n"))
        {
            Name += "\n";
        }
    }

    protected override void UpdateValueLabel()
    {
        if (!valueLabel.text.EndsWith("\n"))
        valueLabel.text += '\n';
    }
}

