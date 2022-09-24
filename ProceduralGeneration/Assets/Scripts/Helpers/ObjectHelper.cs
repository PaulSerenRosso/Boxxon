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
}
