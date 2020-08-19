using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 Round(this Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }

    //rescale it and put it in a different space, snap to that space then scale it up again.
    public static Vector3 Round(this Vector3 v, float size)
    {
        return (v / size).Round() * size; //can't round to any odd numbers
    }

    public static float Round(this float v, float size)
    {
        return Mathf.Round(v / size) * size;
    }

    public static float AtLeast(this float v, float min)
    {
        return Mathf.Max(v, min);
    }

    public static int AtLeast(this int v, int min)
    {
        return Mathf.Max(v, min);
    }
}