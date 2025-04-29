using System.Reflection;
using UnityEngine;

namespace Screensaver;

internal class SelectableScreensaver
{
    private string _name = "";
    private Texture2D _tex;
    private Rect[] _uvs;

    private int _index = 0;
    private float _timePerFrame = 0;

    private float _prevFrameTime = 0;

    internal string Name { get { return _name; } }

    internal Texture2D Texture { get { return _tex; } }

    internal int height { get => (int)(_uvs[_index].height * _tex.height);  }
    internal int width { get => (int)(_uvs[_index].width * _tex.width);  }

    internal SelectableScreensaver(string name, Texture2D texture, Rect[] uvs = null, float frameTime = 0)
    {
        _name = name;
        _tex = texture;
        _uvs = uvs;
        if (_uvs == null)
        {
            _uvs = [new Rect(0, 0, 1, 1)];
        }
        _timePerFrame = frameTime;
        _prevFrameTime = Time.unscaledTime;
    }
    
    internal Rect GetUV()
    {
        if (Time.unscaledTime - _prevFrameTime >= _timePerFrame)
        {
            if (++_index >= _uvs.Length)
                _index = 0;
            _prevFrameTime = Time.unscaledTime;
        }

        return _uvs[_index];
    }
}
