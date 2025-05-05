using System.Reflection;
using Screensaver.Utils;
using UnityEngine;

namespace Screensaver;

internal class SelectableScreensaver
{
    private string _name = "";
    private Texture2D _tex;
    private Rect[] _rects;

    private int _index = 0;
    private float _timePerFrame = 0;

    private float _prevFrameTime = 0;

    internal string Name { get { return _name; } }

    internal Texture2D Texture { get { return _tex; } }

    internal Rect Rect { get { return _rects[_index]; } }

    internal int height { get => (int)(_rects[_index].height * _tex.height);  }
    internal int width { get => (int)(_rects[_index].width * _tex.width);  }

    internal SelectableScreensaver(string name, Texture2D texture, Rect[] rects = null, float frameTime = 0)
    {
        _name = name;
        _tex = texture;
        _rects = rects;
        if (_rects == null)
        {
            _rects = [new Rect(0, 0, 1, 1)];
        }
        _timePerFrame = frameTime;
        _prevFrameTime = Time.unscaledTime;
    }
    
    internal Rect GetUV()
    {
        if (Time.unscaledTime - _prevFrameTime >= _timePerFrame)
        {
            if (++_index >= _rects.Length)
                _index = 0;
            _prevFrameTime = Time.unscaledTime;
        }

        return _rects[_index];
    }
}
