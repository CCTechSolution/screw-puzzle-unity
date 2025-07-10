using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteCollider : MonoBehaviour
{
    [MenuItem("Sprite Collider/Generate Collider")]
    static void GenerateCollider()
    {
        foreach (Transform t in Selection.transforms)
        {
            InitializeSpriteCollider(t.gameObject);
            MakeCollider();
        }
    }

    #region variables
    static PolygonCollider2D polygonCollider2D;
    static Sprite sprite;
    static float spriteWidth;
    static float spriteHeight;
    static float pixelPerUnit;
    static float pixelSide;
    static byte alphaTheshold = 10;
    private static List<Vector2> points = new List<Vector2>();
    static List<Vector2> sortedPoints = new List<Vector2>();
    static List<double> angles = new List<double>();
    static bool addPoint = false;
    #endregion

    static double GetAngle(Vector2 me, Vector2 target)
    {
        return Mathf.Atan2(target.y - me.y, target.x - me.x) * (180 / Mathf.PI);
    }

    static void InitializeSpriteCollider(GameObject go)
    {
        sprite = go.GetComponent<SpriteRenderer>().sprite;
        if (go.GetComponent<PolygonCollider2D>() != null)
        {
            polygonCollider2D = go.GetComponent<PolygonCollider2D>();
        }
        else
        {
            polygonCollider2D = go.AddComponent<PolygonCollider2D>();
        }
        spriteWidth = sprite.rect.width;
        spriteHeight = sprite.rect.height;
        pixelPerUnit = sprite.pixelsPerUnit;
        pixelSide = 1 / pixelPerUnit;
    }

    // Start is called before the first frame update
    static void MakeCollider()
    {
        #region variables
        float pivotX = sprite.pivot.x;
        float pivotY = sprite.pivot.y;
        int minY = 1000000;
        int maxY = 0;
        int j = 0;
        int k = 0;
        #endregion

        for (int x = 0; x <= spriteWidth; x++)
        {
            for (int y = 0; y <= spriteHeight; y++)
            {
                Color32 pixelColor = sprite.texture.GetPixel(x, y);
                if (pixelColor.a > alphaTheshold)
                {
                    if (minY >= y)
                    {
                        minY = y;
                    }
                    if (maxY <= y)
                    {
                        maxY = y;
                    }
                    addPoint = true;
                }
            }
            if (addPoint)
            {
                Vector2 minPoint = new Vector3(x * pixelSide - pivotX * pixelSide, minY * pixelSide - pivotY * pixelSide);
                Vector2 maxPoint = new Vector3(x * pixelSide - pivotX * pixelSide, maxY * pixelSide - pivotY * pixelSide);
                points.Add(minPoint);
                points.Add(maxPoint);
            }
            minY = 1000000;
            maxY = 0;
            addPoint = false;
        }

        foreach (Vector2 point in points)
        {
            angles.Add(GetAngle(Vector2.zero, point));
        }

        int iterations = angles.Count;
        double minAngle = 100000;

        for (int i = 0; i < iterations; i++)
        {
            foreach (double angle in angles)
            {
                if (minAngle > angle)
                {
                    minAngle = angle;
                    j = k;
                }
                k++;
            }
            sortedPoints.Add(points[j]);
            points.Remove(points[j]);
            angles.Remove(minAngle);
            minAngle = 100000;
            k = 0;
        }

        // Simplify the points list
        sortedPoints = SimplifyPolygon.Simplify(sortedPoints, 0.1f);

        polygonCollider2D.SetPath(0, sortedPoints);
        points.Clear();
        sortedPoints.Clear();
        angles.Clear();
    }
}

public static class SimplifyPolygon
{
    public static List<Vector2> Simplify(List<Vector2> points, float tolerance)
    {
        if (points == null || points.Count < 3)
            return points;

        var simplified = new List<Vector2>();
        SimplifyDP(points, tolerance, 0, points.Count - 1, simplified);
        simplified.Add(points[points.Count - 1]);

        return simplified;
    }

    private static void SimplifyDP(List<Vector2> points, float tolerance, int start, int end, List<Vector2> result)
    {
        float maxDistance = 0;
        int index = 0;

        for (int i = start + 1; i < end; i++)
        {
            float distance = PerpendicularDistance(points[start], points[end], points[i]);
            if (distance > maxDistance)
            {
                index = i;
                maxDistance = distance;
            }
        }

        if (maxDistance > tolerance)
        {
            if (index - start > 1)
                SimplifyDP(points, tolerance, start, index, result);
            result.Add(points[index]);
            if (end - index > 1)
                SimplifyDP(points, tolerance, index, end, result);
        }
    }

    private static float PerpendicularDistance(Vector2 start, Vector2 end, Vector2 point)
    {
        float dx = end.x - start.x;
        float dy = end.y - start.y;

        float mag = Mathf.Sqrt(dx * dx + dy * dy);
        if (mag > 0.0f)
        {
            dx /= mag;
            dy /= mag;
        }

        float pvx = point.x - start.x;
        float pvy = point.y - start.y;

        float pvdot = dx * pvx + dy * pvy;

        float ax = pvdot * dx;
        float ay = pvdot * dy;

        float distance = Mathf.Sqrt((pvx - ax) * (pvx - ax) + (pvy - ay) * (pvy - ay));

        return distance;
    }
}
