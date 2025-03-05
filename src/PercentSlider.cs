using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Modding;
using Modding.Menu;
using Satchel.BetterMenus;

namespace Screensaver;

public class PercentSlider : CustomSlider
{
    public PercentSlider(string name, Action<float> storeValue, Func<float> loadValue, float minValue, float maxValue, bool wholeNumbers = false, string Id = "__UseName")
        : base(name, storeValue, loadValue, minValue, maxValue, wholeNumbers, Id)
    {
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

    private void UpdateValueLabel()
    {
        valueLabel.text = $"{value * 100:0}%";
    }
}
