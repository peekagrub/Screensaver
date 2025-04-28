using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Screensaver;

internal class ScreensaverBehaviour : MonoBehaviour
{
    private Vector2 position;
    private Vector2 direction;

    private Color col = Color.grey;

    private event Action onBounce;

    private void Start()
    {
        float maxDim = Mathf.Max(Screen.width, Screen.height);

        float clampedSize = Mathf.Clamp(maxDim * Screensaver.settings.screenPercentage, 0, Mathf.Min(Screen.width, Screen.height));
        position = new Vector2(Random.Range(0, Screen.width - clampedSize), Random.Range(0, Screen.height - clampedSize));

        direction = new Vector2(Random.value < 0.5 ? 1 : -1, Random.value < 0.5 ? 1 : -1);
    }
    

    internal void ToggleScreensaver(bool enabled)
    {
        base.enabled = enabled;
    }

    internal void ToggleColorOnBounce(bool change)
    {
        if (change) 
        {
            onBounce += randomColor;
        }
        else
        {
            onBounce -= randomColor;
            col = Color.grey;
        }
    }

    private void randomColor()
    {
        col = Random.ColorHSV();
    }

    private void OnGUI()
    {
        if (Event.current?.type != EventType.Repaint)
        {
            return;
        }

        SelectableScreensaver ss = ScreensaverManager.Instance.CurrentScreensaver;

        Rect uv = ss.GetUv();

        float maxDim = Mathf.Max(Screen.width, Screen.height);

        position += direction * Time.unscaledDeltaTime * maxDim * Screensaver.settings.speed;

        float clampedSize = Mathf.Clamp(maxDim * Screensaver.settings.screenPercentage, 0, Mathf.Min(Screen.width, Screen.height));
        Vector2 size = new Vector2(clampedSize, clampedSize);


        if (ss.width > ss.height)
        {
            size.x *= ss.width / (float)ss.height;
        }
        else if (ss.width < ss.height)
        {
            size.y *= ss.height / (float)ss.width;
        }

        if ((direction.x > 0 && position.x + size.x >= Screen.width) || (direction.x < 0 && position.x <= 0))
        {
            direction.x *= -1;
            onBounce?.Invoke();
        }
        if ((direction.y > 0 && position.y + size.y >= Screen.height) || (direction.y < 0 && position.y <= 0))
        {
            direction.y *= -1;
            onBounce?.Invoke();
        }

        float xPos = position.x;
        float yPos = position.y;
        float xSize = size.x;
        float ySize = size.y;

        Graphics.DrawTexture(new Rect(xPos, yPos, xSize, ySize), ss.Texture, uv, 0, 0, 0, 0, col);
    }
}
