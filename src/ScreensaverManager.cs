using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Satchel;
using Newtonsoft.Json;
using UnityEngine;
using UObject = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Screensaver;

internal class ScreensaverManager
{
    internal static string SCREENSAVERS_FOLDER;

    private static ScreensaverManager _instance;

    internal static ScreensaverManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ();
            }

            return _instance;
        }
    }

    private SelectableScreensaver _default;
    private SelectableScreensaver _currentScreensaver;

    internal SelectableScreensaver CurrentScreensaver 
    { 
        get 
        { 
            if (_currentScreensaver == null)
            {
                _currentScreensaver = _default;
            }
            return _currentScreensaver; 
        }
    }

    internal List<SelectableScreensaver> ScreensaversList;

    static ScreensaverManager()
    {
        SCREENSAVERS_FOLDER = Path.Combine(AssemblyUtils.getCurrentDirectory(), "Screensavers");
    }

    private ScreensaverManager() 
    {
        ReloadScreensavers();
    }

    internal void SetScreensaverByIndex(int index)
    {
        if (index >= ScreensaversList.Count)
        {
            SetScreensaverByName("Default");
        } else
        {
            _currentScreensaver = ScreensaversList[index];
            Menus.ModMenu.selectedSaver = index;
        }
    }

    internal void SetScreensaverByName(string name)
    {
        _currentScreensaver = ScreensaversList.FirstOrDefault(s => s.Name == name, _default);
        Menus.ModMenu.selectedSaver = ScreensaversList.FindIndex(s => s == _currentScreensaver);
    }

    internal void ReloadScreensavers()
    {
        ScreensaversList = Directory.GetDirectories(SCREENSAVERS_FOLDER)
            .Select(LoadScreensaverDirectory)
            .Where(s => s != null)
            .ToList();

        _default = ScreensaversList.Where(s => s.Name == "Default").First();
        SetScreensaverByName(CurrentScreensaver.Name);
    }

    private SelectableScreensaver LoadScreensaverDirectory(string dirPath)
    {
        Screensaver.Instance.Log($"Loading {dirPath}");
        string jsonPath = Path.Combine(dirPath, "screensaver.json");
        try {
            if (File.Exists(jsonPath))
            {
                SaverSettings saver = JsonConvert.DeserializeObject<SaverSettings>(File.ReadAllText(jsonPath));
                if (saver.IsAnimated)
                {
                    return LoadAnimatedScreensaver(dirPath, saver.FileNames, saver.FPS, saver.FrameTime);
                } else if (saver.FileNames != null)
                {
                    byte[] bytes = File.ReadAllBytes(Path.Combine(dirPath, saver.FileNames[0]));
                    Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    tex.LoadImage(bytes);

                    return new SelectableScreensaver(new DirectoryInfo(dirPath).Name, tex);
                }
                
                Screensaver.Instance.LogError($"Exception trying to load {new DirectoryInfo(dirPath).Name}");
                return null;
            } else
            {
                string file = Directory.GetFiles(dirPath).Where(f => Regex.IsMatch(f, @"\.jpe?g$|\.png$")).First();

                byte[] bytes = File.ReadAllBytes(file);
                Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                tex.LoadImage(bytes);

                return new SelectableScreensaver(new DirectoryInfo(dirPath).Name, tex);
            }
        } catch (Exception e)
        {
            Screensaver.Instance.LogError($"Exception trying to load {new DirectoryInfo(dirPath).Name}: {e}");
            return null;
        }
    }

    private SelectableScreensaver LoadAnimatedScreensaver(string dirPath, List<string> fileNames, float fps, float frameTime)
    {
        if (fileNames != null)
        {
            Texture2D[] texs = fileNames
                .Select(f => {
                    byte[] bytes = File.ReadAllBytes(Path.Combine(dirPath, f));
                    Texture2D newTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    newTexture.LoadImage(bytes);
                    return newTexture;
                })
                .ToArray();

            int sheetSize = texs.Sum(t => t.width);

            Texture2D atlas = new Texture2D(1, 1);

            Rect[] rects = atlas.PackTextures(texs, 0, sheetSize, true);
            if (rects == null)
            {
                Screensaver.Instance.LogError($"Error packing textures for {new DirectoryInfo(dirPath).Name}");
            }
            atlas.Compress(true);

            foreach(var tex in texs)
            {
                UObject.Destroy(tex);
            }

            frameTime = frameTime == 0.0f ? 1 / fps : frameTime;

            return new SelectableScreensaver(new DirectoryInfo(dirPath).Name, atlas, rects, frameTime);
        }

        Screensaver.Instance.LogError($"Error trying to load {new DirectoryInfo(dirPath).Name}");
        return null;
    }
}

