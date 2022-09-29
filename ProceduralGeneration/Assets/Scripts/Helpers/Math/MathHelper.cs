using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MathHelper
{
    public static bool IsClamp(this float _value, float _min, float _max)
    {
        if (_value >= _min && _value <= _max)
        {
            return true;
        }
        return false;
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

   public static Circle GetTriangleCircumcircle(Triangle _triangle)
   {
       return new Circle();
   }
   
   public static Triangle GetTriangleWitchInscribesRect(Rect _rect, float _sizeOfTriangleBaseOffset)
   {
       return new Triangle();
   }
}
