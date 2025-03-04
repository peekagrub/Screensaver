using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Screensaver;

public class ScreensaverBehaviour: MonoBehaviour
{
    private Texture2D ssTexture;
    private Vector2 position;
    private Vector2 direction;

    private void Start()
    {
        ssTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        ssTexture.SetPixel(0, 1, Color.white);
        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Screensaver.Resources.screensaver.png"))
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            ssTexture.LoadImage(bytes);
        }
        ssTexture.Apply();

        float maxDim = Mathf.Max(Screen.width, Screen.height);
        float clampedSize = Mathf.Clamp(maxDim * 0.25f, 0, Mathf.Min(Screen.width, Screen.height));
        position = new Vector2(Random.Range(0, Screen.width - clampedSize), Random.Range(0, Screen.height - clampedSize));

        direction = new Vector2(Random.value < 0.5 ? 1 : -1, Random.value < 0.5 ? 1 : -1);
    }

    private void OnGUI()
    {
        if (Event.current?.type != EventType.Repaint)
        {
            return;
        }

        float maxDim = Mathf.Max(Screen.width, Screen.height);

        position += direction * Time.unscaledDeltaTime * (maxDim / 6);

        float clampedSize = Mathf.Clamp(maxDim * 0.25f, 0, Mathf.Min(Screen.width, Screen.height));
        Vector2 size = new Vector2(clampedSize, clampedSize);

        if ((direction.x > 0 && position.x + size.x >= Screen.width) || (direction.x < 0 && position.x <= 0))
        {
            direction.x *= -1;
        }
        if ((direction.y > 0 && position.y + size.y >= Screen.height) || (direction.y < 0 && position.y <= 0))
        {
            direction.y *= -1;
        }

        float xPos = position.x;
        float yPos = position.y;
        float xSize = size.x;
        float ySize = size.y;

        Rect rect = new Rect(xPos, yPos, xSize, ySize);

        GUI.DrawTexture(rect, ssTexture, ScaleMode.ScaleToFit);
    }
}
