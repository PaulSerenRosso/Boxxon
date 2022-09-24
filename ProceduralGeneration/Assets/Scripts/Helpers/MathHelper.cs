using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MathHelper
{

    public static bool IsClamp(this float value, float min, float max)
    {
        if (value >= min && value <= max)
        {
            return true;
        }
        return false;
    }

    public static bool IsClamp(this int value, int min, int max)
    {
        if (value >= min && value <= max)
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
    
   public static bool InMinDistance(this Vector2 pointA, Vector2 pointB, float minDistance)
    {
        float distanceBetweenPoints = (pointA - pointB).sqrMagnitude;
        if (distanceBetweenPoints < minDistance*minDistance)
        {
            return true;
        }

        return false; 
    }
   
}
