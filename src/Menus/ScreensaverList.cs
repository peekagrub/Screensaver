using Screensaver;
using Satchel.BetterMenus;

namespace Screensaver.Menus;

internal static class ScreensaverList
{
    internal static Menu MenuRef;
    internal static MenuScreen lastMenu;

    private static MenuButton ApplyScreensaverButton(int index)
    {
        string buttonText = MaxLength(ScreensaverManager.Instance.ScreensaversList[index].Name, 15);
        return new MenuButton(buttonText, "",
            (mb) => {
                ModMenu.ApplyScreensaver(index);
                UIManager.instance.UIGoToDynamicMenu(lastMenu);
            },
            Id:$"skinbutton_{ScreensaverManager.Instance.ScreensaversList[index].Name}"
        );
    }

    private static string MaxLength(string name, int length)
    {
        return name.Length <= length ? name : name.Substring(0, length - 3) + "...";
    }

    private static Menu PrepareMenu()
    {
        Menu menu = new Menu("Select a screensaver", new Element[]{
            new TextPanel("Select a Screensaver to Apply", Id:"helptext"),
        });

        for (var i = 0; i < ScreensaverManager.Instance.ScreensaversList.Count; i++)
        {
            menu.AddElement(ApplyScreensaverButton(i));
        }

        return menu;
    }

    internal static MenuScreen GetMenu(MenuScreen lastMenu)
    {
        MenuRef ??= PrepareMenu();

        ScreensaverList.lastMenu = lastMenu;

        return MenuRef.GetCachedMenuScreen(lastMenu);
    }
}
