using System;
using UnityEngine;
using UnityEngine.UI;
using Modding;
using Modding.Menu;
using Satchel.BetterMenus;

namespace Screensaver;

public class FixedCustomSlider : CustomSlider
{
    public FixedCustomSlider(string name, Action<float> storeValue, Func<float> loadValue, float minValue, float maxValue, bool wholeNumbers = false, string Id = "__UseName")
        : base(name, storeValue, loadValue, minValue, maxValue, wholeNumbers, Id)
    {
    }

    public override GameObjectRow Create(ContentArea c, Menu Instance, bool AddToList = true)
    {
        var ret = base.Create(c, Instance, AddToList);
        Action<float> updateOnEvent = newValue =>
        {
            UpdateValueLabel();
            label.text = Name + '\n';
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

