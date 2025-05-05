using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Screensaver;

internal class ScreensaverBehaviour : MonoBehaviour
{
    private static Rect _rect = new Rect(0, 0, 1, 1);

    private List<Vector2> positions;
    private List<Vector2> directions;
    private List<Color> colors;

    private event Action<int> onBounce;

    private void Awake()
    {
        float maxDim = Mathf.Max(Screen.width, Screen.height);
        float clampedSize = Mathf.Clamp(maxDim * Screensaver.settings.screenPercentage, 0, Mathf.Min(Screen.width, Screen.height));

        positions = new List<Vector2>(Screensaver.settings.count);
        directions = new List<Vector2>(Screensaver.settings.count);
        colors = new List<Color>(Screensaver.settings.count);

        for (int i = 0; i < positions.Capacity; i++)
        {
            positions.Add(new Vector2(Random.Range(0, Screen.width - clampedSize), Random.Range(0, Screen.height - clampedSize)));
            directions.Add(new Vector2(Random.value < 0.5 ? 1 : -1, Random.value < 0.5 ? 1 : -1));
            colors.Add(Color.grey);
        }
    }

    internal void ToggleScreensaver(bool _enabled)
    {
        enabled = _enabled;
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
            for (int i = 0; i < colors.Count; i++)
            {
                colors[i] = Color.grey;
            }
        }
    }

    internal void SetCount(int newCount)
    {
        if (newCount < positions.Count)
        {
            positions.RemoveRange(newCount, positions.Count - newCount);
            directions.RemoveRange(newCount, directions.Count - newCount);
            colors.RemoveRange(newCount, colors.Count - newCount);
        } else if (newCount > positions.Count)
        {
            float maxDim = Mathf.Max(Screen.width, Screen.height);
            float clampedSize = Mathf.Clamp(maxDim * Screensaver.settings.screenPercentage, 0, Mathf.Min(Screen.width, Screen.height));

            for (int i = positions.Count; i < newCount; i++)
            {
                positions.Add(new Vector2(Random.Range(0, Screen.width - clampedSize), Random.Range(0, Screen.height - clampedSize)));
                directions.Add(new Vector2(Random.value < 0.5 ? 1 : -1, Random.value < 0.5 ? 1 : -1));
                colors.Add(Color.grey);
            }
        }
    }

    private void randomColor(int index)
    {
        colors[index] = Random.ColorHSV();
    }

    private void OnGUI()
    {
        if (Event.current?.type != EventType.Repaint)
        {
            return;
        }

        SelectableScreensaver ss = ScreensaverManager.Instance.CurrentScreensaver;

        Rect uv = ss.GetUV();

        float maxDim = Mathf.Max(Screen.width, Screen.height);

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

        Matrix4x4 scale = Matrix4x4.Scale(size);

        GL.PushMatrix();
        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 position = positions[i];
            Vector2 direction = directions[i];
            position += direction * Time.unscaledDeltaTime * maxDim * Screensaver.settings.speed;

            if ((direction.x > 0 && position.x + size.x >= Screen.width) || (direction.x < 0 && position.x <= 0))
            {
                direction.x *= -1;
                onBounce?.Invoke(i);
            }
            if ((direction.y > 0 && position.y + size.y >= Screen.height) || (direction.y < 0 && position.y <= 0))
            {
                direction.y *= -1;
                onBounce?.Invoke(i);
            }

            GL.MultMatrix(Matrix4x4.Translate(position) * scale);

            Graphics.DrawTexture(_rect, ss.Texture, uv, 0, 0, 0, 0, colors[i]);

            positions[i] = position;
            directions[i] = direction;
        }
        GL.PopMatrix();
    }
}
