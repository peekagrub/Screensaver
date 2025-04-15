using System;
using Modding;
using Modding.Menu;
using Satchel.BetterMenus;

namespace Screensaver;

public class FixedCustomSlider : CustomSlider
{
    public FixedCustomSlider(string name, Action<float> storeValue, Func<float> loadValue, string Id = "__UseName") 
        : base(name, storeValue, loadValue, Id)
    {
        if (!Name.EndsWith("\n"))
        {
            Name += "\n";
        }
    }

    public FixedCustomSlider(string name, Action<float> storeValue, Func<float> loadValue, float minValue, float maxValue, bool wholeNumbers = false, string Id = "__UseName")
        : base(name, storeValue, loadValue, minValue, maxValue, wholeNumbers, Id)
    {
        if (!Name.EndsWith("\n"))
        {
            Name += "\n";
        }
    }

    public override GameObjectRow Create(ContentArea c, Menu Instance, bool AddToList = true)
    {
        var ret = base.Create(c, Instance, AddToList);
        Action<float> updateOnEvent = newValue =>
        {
            UpdateValueLabel();
        };

        slider.onValueChanged.AddListener(updateOnEvent.Invoke);

        return ret;
    }

    public override void Update()
    {
        base.Update();
        UpdateValueLabel();
    }

    protected virtual void UpdateValueLabel()
    {
        if (!valueLabel.text.EndsWith("\n"))
        valueLabel.text += '\n';
    }
}

