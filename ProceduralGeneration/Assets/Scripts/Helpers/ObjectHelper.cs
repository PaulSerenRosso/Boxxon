using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectHelper 
{
    public static void SetGlobalScale(this Transform _transform, Vector3 newScale)
    {
        Transform parent = _transform.parent;
        _transform.parent = null;
        _transform.localScale = newScale;
        _transform.parent = parent;
    }

    public static void SetGlobalScale(this Transform _transform, Coordinate[] _coordinates)
    {
        Transform parent = _transform.parent;
        _transform.parent = null;
        Vector3 transformScale = _transform.localScale;
        for (int i = 0; i < _coordinates.Length; i++)
        {
        switch (_coordinates[i].type)
        {
            case CoordinateType.X:
            {
                transformScale.x = _coordinates[i].value;
                break;
            }
            case CoordinateType.Y:
            {
                transformScale.y = _coordinates[i].value;
                break;
            }
            case CoordinateType.Z:
            {
                transformScale.z = _coordinates[i].value;
                break;
            }
        }
        }
        _transform.localScale = transformScale;
        _transform.parent = parent;
    }

    public static Vector3 ConvertTo3dSpace(this Vector2 position2d, CoordinateType xConverter , CoordinateType yConverter, Vector3 offset)
    {
        Coordinate[] coordinatesToConvert = new[] { new Coordinate(xConverter,position2d.x), new Coordinate(yConverter, position2d.y)};
        
        Vector3 result = Vector3.zero;
        for (int i = 0; i < coordinatesToConvert.Length; i++)
        {
            switch (coordinatesToConvert[i].type)
            {
                case CoordinateType.X :
                {
                    result.x = coordinatesToConvert[i].value;
                 break;   
                }
                case CoordinateType.Y : 
                {
                    result.y = coordinatesToConvert[i].value;
                    break; 
                }
                case CoordinateType.Z : 
                {
                    result.z = coordinatesToConvert[i].value;
                    break; 
                }
            }
        }
        result += offset;
        return result;
    }
}
