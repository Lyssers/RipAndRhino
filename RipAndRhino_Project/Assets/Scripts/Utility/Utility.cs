using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void DeleteChildren(this Transform parent)
    {
        foreach (Transform item in parent)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}