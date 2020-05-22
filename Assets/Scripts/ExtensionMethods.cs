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
}
