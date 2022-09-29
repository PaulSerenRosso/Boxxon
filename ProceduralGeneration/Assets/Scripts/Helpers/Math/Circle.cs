using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct Circle 
{
    public float3 center;
    public float3 radius;

    public Circle(float3 _center, float3 _radius)
    {
        center = _center;
        radius = _radius;
    }
}
