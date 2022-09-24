using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoordinateType
{
X,Y,Z
}

public class Coordinate
{
    public Coordinate(CoordinateType _type, float _value)
    {
        type = _type;
        value = _value;
    }
    public CoordinateType type;
    public float value;
}
