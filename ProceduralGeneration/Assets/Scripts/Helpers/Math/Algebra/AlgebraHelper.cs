using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

static public class AlgebraHelper
{
    public static bool IsClamp(this float _value, float _min, float _max)
    {
        if (_value >= _min && _value <= _max)
        {
            return true;
        }
        return false;
    }
    
    public static bool isClamp(this Vector3 _inputVec, Vector3 _minVec, Vector3 _maxVec)
    {
        return (IsClamp(_inputVec.x, _maxVec.x, _minVec.x) && IsClamp(_inputVec.y, _maxVec.y, _minVec.y) 
                                                        && IsClamp(_inputVec.z, _maxVec.z, _minVec.z));
    }
    
    public static Vector3 Clamp(this Vector3 _inputVec, Vector3 _minVec, Vector3 _maxVec)
    {
        _inputVec.x = Mathf.Clamp(_inputVec.x, _maxVec.x, _minVec.x);
        _inputVec.y = Mathf.Clamp(_inputVec.y, _maxVec.y, _minVec.y);
        _inputVec.z = Mathf.Clamp(_inputVec.z, _maxVec.z, _minVec.z);
        return _inputVec;
    }

    public static bool IsClamp(this int _value, int _min, int _max)
    {
        if (_value >= _min && _value <= _max)
        {
            return true; 
        }
        return false;
    }

    public static Vector2 RandomDirection()
    {
        float angle = Random.value * 2 * Mathf.PI;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    
   public static bool InMinDistance(this Vector2 _pointA, Vector2 _pointB, float _minDistance)
    {
        float distanceBetweenPoints = (_pointA - _pointB).sqrMagnitude;
        if (distanceBetweenPoints < _minDistance*_minDistance)
        {
            return true;
        }

        return false; 
    }


}
