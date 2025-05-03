using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ExtensionMethods
{
    //public static void TriggerStartActionIfCondition(Func<bool> condition, Action onCondition)
    //{
    //    ReferenceManager.instance.StartCoroutine(StartActionIfCondition(condition, onCondition));
    //}

    static IEnumerator StartActionIfCondition(Func<bool> condition, Action onCondition)
    {
        while (!condition.Invoke())
        {
            yield return null;
        }
        onCondition.Invoke();
    }

    public static float Map(this float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public static void SetLayerRecursively(this GameObject obj, int newLayer)
    {
        if (obj == null) return; // Safety check

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer); // Recursively set layer for each child
        }
    }

    public static void SetGameobjectActiveState(this List<GameObject> list, bool state)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(state);
        }
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Vector3 XZ(this Vector3 value)
    {
        value.y = 0;
        return value;
    }

    public static float CheapDistance(this Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
    }

    public static Vector3 Dir(this Vector3 start, Vector3 target)
    {
        return target - start;
    }

    public static Vector3 DirCopyY(this Vector3 start, Vector3 target)
    {
        target.y = start.y;
        return target - start;
    }

    public static Vector3 SetY(this Vector3 value, float y)
    {
        value.y = y;
        return value;
    }

    public static Vector3 AddY(this Vector3 value, float y)
    {
        value.y += y;
        return value;
    }
    /// <summary>
    /// Positive AND! Negative
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static Vector3 CreateFromBiggestNumber(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3();
        c.x = Mathf.Abs(a.x) < Mathf.Abs(b.x) ? b.x : a.x;
        c.y = Mathf.Abs(a.y) < Mathf.Abs(b.y) ? b.y : a.y;
        c.z = Mathf.Abs(a.z) < Mathf.Abs(b.z) ? b.z : a.z;
        return c;
    }

    public static Vector3 CreateFromSmallestNumber(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3();
        c.x = Mathf.Abs(a.x) > Mathf.Abs(b.x) ? b.x : a.x;
        c.y = Mathf.Abs(a.y) > Mathf.Abs(b.y) ? b.y : a.y;
        c.z = Mathf.Abs(a.z) > Mathf.Abs(b.z) ? b.z : a.z;
        return c;
    }

    public static Vector3 Minus(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vector3 RandomPointInXZBounds(this Collider collider)
    {
        return new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(0, 1),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );
    }

}