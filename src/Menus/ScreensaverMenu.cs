using Modding;
using Screensaver;
using Satchel.BetterMenus;

namespace Screensaver.Menus;

internal static class ScreensaverMenu
{
    internal static Menu MenuRef;

    private static Menu PrepareMenu()
    {
        return new Menu("Screensaver Options", new Element[]{
            Blueprints.HorizontalBoolOption("Change color on bounce", 
                "Toggle if the color of the screensaver should change on bounce",
                (option) => { Screensaver.settings.changeColorOnBounce = option; Screensaver.Instance.UpdateColorOnBounce(); },
                () => Screensaver.settings.changeColorOnBounce
            ),
            new FixedCustomSlider("Screensaver Count",
                (option) => { Screensaver.settings.count = (int)option; Screensaver.Instance.UpdateCount(); },
                () => Screensaver.settings.count,
                1f, 25.0f, true
            ),
            new PercentSlider("Screen Percentage",
                (option) => Screensaver.settings.screenPercentage = option,
                () => Screensaver.settings.screenPercentage,
                0.01f, 0.5f, false
            ),
            new FixedCustomSlider("Screensaver Speed",
                (option) => Screensaver.settings.speed = option,
                () => Screensaver.settings.speed,
                0.1f, 2.0f, false
            ),
        });
    }

    internal static MenuScreen GetMenu(MenuScreen lastMenu)
    {
        MenuRef ??= PrepareMenu();

        return MenuRef.GetCachedMenuScreen(lastMenu);
    }
}
