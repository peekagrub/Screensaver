using System.Linq;
using System.Collections.Generic;
using Modding;
using Screensaver;
using Satchel;
using Satchel.BetterMenus;

namespace Screensaver.Menus;

internal static class ModMenu
{
    internal static Menu MenuRef;
    internal static int selectedSaver;

    private static string[] getScreensaverNameArray()
    {
        return ScreensaverManager.Instance.ScreensaversList.Select(s => s.Name).ToArray();
    }

    internal static void ApplyScreensaver(int index)
    {
        selectedSaver = index;
        ScreensaverManager.Instance.SetScreensaverByIndex(selectedSaver);
    }

    private static void OpenScreensavers()
    {
        IoUtils.OpenDefault(ScreensaverManager.SCREENSAVERS_FOLDER);
    }

    private static Menu PrepareMenu()
    {
        return new Menu("Screensaver", new Element[]{
            Blueprints.HorizontalBoolOption("Screensaver",
                "Toggle if the screensaver is active",
                (option) => { Screensaver.settings.enabled = option; Screensaver.Instance.UpdateToggle(); },
                () => Screensaver.settings.enabled
            ),
            new HorizontalOption("Select Screensaver",
                "The screensaver will be used for all instances.",
                getScreensaverNameArray(),
                ApplyScreensaver,
                () => selectedSaver
            ),
            new TextPanel("To Add more screensavers, copy the screensavers into your Screensavers folder."),
            new MenuRow(
                new List<Element>{
                    Blueprints.NavigateToMenu("Screensaver List",
                        "Opens a list of Screensavers",
                        () => ScreensaverList.GetMenu(MenuRef.menuScreen)
                    ),
                    Blueprints.NavigateToMenu("Screensaver Options",
                        "Screensaver Options",
                        () => ScreensaverMenu.GetMenu(MenuRef.menuScreen)
                    ),
                },
                Id:"SubMenuGroup"
            ),
            new MenuRow(
                new List<Element>{
                    new MenuButton("Reload",
                        "Reloads all screensavers",
                        (_) => ScreensaverManager.Instance.ReloadScreensavers()
                    ),
                    new MenuButton("Open Folder",
                        "Open screensavers folder",
                        (_) => OpenScreensavers()
                    )
                },
                Id:"HelpButtonGroup"
            ),
        });
    }

    internal static MenuScreen GetMenu(MenuScreen lastMenu, ModToggleDelegates? toggleDelegates)
    {
        MenuRef ??= PrepareMenu();

        return MenuRef.GetMenuScreen(lastMenu);
    }
}
