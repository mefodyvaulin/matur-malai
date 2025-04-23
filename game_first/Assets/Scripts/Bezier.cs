using UnityEngine;

public static class Bezier
{
    /// <summary>
    /// Кривая Безье второго порядка (3 точки: P0, P1, P2)
    /// </summary>
    public static Vector3 Quadratic(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }

    /// <summary>
    /// Кривая Безье третьего порядка (4 точки: P0, P1, P2, P3)
    /// </summary>
    public static Vector3 Cubic(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        var u = 1 - t;
        var tt = t * t;
        var uu = u * u;
        var uuu = uu * u;
        var ttt = tt * t;

        var point = uuu * p0; // (1 - t)^3 * P0
        point += 3 * uu * t * p1;    // 3 * (1 - t)^2 * t * P1
        point += 3 * u * tt * p2;    // 3 * (1 - t) * t^2 * P2
        point += ttt * p3;           // t^3 * P3

        return point;
    }
}

