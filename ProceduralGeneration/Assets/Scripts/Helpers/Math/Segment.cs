using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Segment
{

    public Segment(Vector2 _pointA, Vector2 _pointB)
    {
        PointA = _pointA;
        PointB = _pointB;
    }
    public Vector2 PointA;
    public Vector2 PointB;
}
