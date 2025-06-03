using System.Collections.Generic;
using UnityEngine;

public class PolygonColliderConvexGenerator
{
    public bool GenerateConvexFromAlpha(SpriteRenderer spriteRenderer, PolygonCollider2D collider, List<Vector2> prevHull, out List<Vector2> newHull, float alphaThreshold = 0.1f, int precision = 16)
    {
        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;
        Rect rect = sprite.rect;
        newHull = null;

        int xMin = (int)rect.x;
        int yMin = (int)rect.y;
        int width = (int)rect.width;
        int height = (int)rect.height;
        Vector2 pivot = sprite.pivot;

        List<Vector2> points = new();

        for (int y = 0; y < height; y += precision)
        {
            for (int x = 0; x < width; x += precision)
            {
                Color color = texture.GetPixel(xMin + x, yMin + y);
                if (color.a >= alphaThreshold)
                {
                    float px = (x - pivot.x) / sprite.pixelsPerUnit;
                    float py = (y - pivot.y) / sprite.pixelsPerUnit;
                    points.Add(new Vector2(px, py));
                }
            }
        }

        if (points.Count < 3)
        {
            collider.pathCount = 0;
            return false;
        }

        newHull = ConvexHull(points);

        if (prevHull == null ||
            !AreHullsEqual(prevHull, newHull))
        {
            collider.pathCount = 1;
            collider.SetPath(0, newHull);
            return true;
        }
        return false;
    }

    private bool AreHullsEqual(List<Vector2> a, List<Vector2> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }

        for (int i = 0; i < a.Count; i++)
        {
            if (Vector2.SqrMagnitude(a[i] - b[i]) > 0.0001f)
                return false;
        }
        return true;
    }

    private List<Vector2> ConvexHull(List<Vector2> points)
    {
        points.Sort((a, b) =>
        {
            int cmpX = a.x.CompareTo(b.x);
            return cmpX != 0 ? cmpX : a.y.CompareTo(b.y);
        });

        List<Vector2> lower = new();
        foreach (Vector2 p in points)
        {
            while (lower.Count >= 2 && Cross(lower[^2], lower[^1], p) <= 0)
            {
                lower.RemoveAt(lower.Count - 1);
            }
            lower.Add(p);
        }

        List<Vector2> upper = new();
        for (int i = points.Count - 1; i >= 0; i--)
        {
            Vector2 p = points[i];
            while (upper.Count >= 2 && Cross(upper[^2], upper[^1], p) <= 0)
            {
                upper.RemoveAt(upper.Count - 1);
            }
            upper.Add(p);
        }

        lower.RemoveAt(lower.Count - 1);
        upper.RemoveAt(upper.Count - 1);
        lower.AddRange(upper);
        return lower;
    }

    private static float Cross(Vector2 o, Vector2 a, Vector2 b)
    {
        return (a.x - o.x) * (b.y - o.y) - (a.y - o.y) * (b.x - o.x);
    }
}