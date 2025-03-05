using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Screensaver;

public class ScreensaverBehaviour : MonoBehaviour
{
    private Texture2D ssTexture;
    private Vector2 position;
    private Vector2 direction;

    private Rect rect = new Rect(0, 0, 1, 1);
    private Color col;

    private void Start()
    {
        ssTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        ssTexture.SetPixel(0, 1, Color.white);
        ssTexture.Apply();

        string dirPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        string screenSaversPath = Path.Combine(dirPath, "Screensavers");
        foreach (string file in Directory.GetFiles(screenSaversPath))
        {
            if (file.EndsWith(".png") || file.EndsWith(".jpg")){
                byte[] bytes = File.ReadAllBytes(file);
                ssTexture.LoadImage(bytes);
                break;
            }
        }

        float maxDim = Mathf.Max(Screen.width, Screen.height);

        float clampedSize = Mathf.Clamp(maxDim * Screensaver.settings.screenPercentage, 0, Mathf.Min(Screen.width, Screen.height));
        position = new Vector2(Random.Range(0, Screen.width - clampedSize), Random.Range(0, Screen.height - clampedSize));

        direction = new Vector2(Random.value < 0.5 ? 1 : -1, Random.value < 0.5 ? 1 : -1);

        col = Random.ColorHSV();
    }

    public void ToggleScreensaver(bool enabled)
    {
        base.enabled = enabled;
    }

    private void OnGUI()
    {
        if (Event.current?.type != EventType.Repaint)
        {
            return;
        }

        float maxDim = Mathf.Max(Screen.width, Screen.height);

        position += direction * Time.unscaledDeltaTime * (maxDim / 6);

        float clampedSize = Mathf.Clamp(maxDim * Screensaver.settings.screenPercentage, 0, Mathf.Min(Screen.width, Screen.height));
        Vector2 size = new Vector2(clampedSize, clampedSize);


        if (ssTexture.width > ssTexture.height)
        {
            size.x *= ssTexture.width / (float)ssTexture.height;
        }
        else if (ssTexture.width < ssTexture.height)
        {
            size.y *= ssTexture.height / (float)ssTexture.width;
        }

        if ((direction.x > 0 && position.x + size.x >= Screen.width) || (direction.x < 0 && position.x <= 0))
        {
            direction.x *= -1;
            col = Random.ColorHSV();
        }
        if ((direction.y > 0 && position.y + size.y >= Screen.height) || (direction.y < 0 && position.y <= 0))
        {
            direction.y *= -1;
            col = Random.ColorHSV();
        }

        float xPos = position.x;
        float yPos = position.y;
        float xSize = size.x;
        float ySize = size.y;

        Graphics.DrawTexture(new Rect(xPos, yPos, xSize, ySize), ssTexture, rect, 0, 0, 0, 0, col);
    }
}
