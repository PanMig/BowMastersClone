using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IInputHandling : MonoBehaviour
{
    public delegate void OnShootFired();
    public OnShootFired ONShootFired;
    public abstract Vector3 GetDirectionToTarget(Rigidbody2D rb);
}
