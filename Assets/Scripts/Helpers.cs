using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static T FindComponentInChildWithTag<T>(GameObject parent, string tag)where T:Component
    {
        Transform t = parent.transform;
        foreach(Transform tr in t)
        {
            if(tr.CompareTag(tag))
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }
}
