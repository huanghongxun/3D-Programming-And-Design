using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 工具函数
/// </summary>
public static class Utility {

    private static System.Random rnd = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
public class VectorComparator
{
    public static bool Less(Vector2 a, Vector2 b)
    {
        return a.x < b.x && a.y < b.y;
    }

    public static bool Greater(Vector2 a, Vector2 b)
    {
        return a.x > b.x && a.y > b.y;
    }
    public static bool LessEqual(Vector2 a, Vector2 b)
    {
        return a.x <= b.x && a.y <= b.y;
    }

    public static bool GreaterEqual(Vector2 a, Vector2 b)
    {
        return a.x >= b.x && a.y >= b.y;
    }
}