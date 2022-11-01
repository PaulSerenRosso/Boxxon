using System;
using UnityEngine;
[Serializable]
public struct Circle 
{
    public Vector2 center;
    public float radius;

    public Circle(Vector2 _center, float _radius)
    {
        center = _center;
        radius = _radius;
    }
}
