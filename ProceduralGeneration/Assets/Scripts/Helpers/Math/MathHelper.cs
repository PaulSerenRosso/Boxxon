using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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

   public static Circle GetTriangleCircumCircle(this Triangle _triangle)
   {
       float angleBAC = _triangle.GetTriangleVerticeAngle(0);
       float angleABC = _triangle.GetTriangleVerticeAngle(1);
       float angleBCA = Mathf.PI - (angleABC + angleBAC);
       float doubleSinAngleBAC = Mathf.Sin(angleBAC)*2; 
       float doubleSinAngleABC = Mathf.Sin(angleABC)*2; 
       float doubleSinAngleBCA = Mathf.Sin(angleBCA)*2; 
       float circumCircleRadius = angleBAC/doubleSinAngleBAC;
       float circumCircleCenterX = (_triangle.vertexA.x * doubleSinAngleBAC + _triangle.vertexB.x * doubleSinAngleABC +
                                   _triangle.vertexC.x * doubleSinAngleBCA)/(doubleSinAngleABC+doubleSinAngleBAC+doubleSinAngleBCA);
       float circumCircleCenterY = (_triangle.vertexA.y * doubleSinAngleBAC + _triangle.vertexB.y * doubleSinAngleABC +
                                    _triangle.vertexC.y * doubleSinAngleBCA)/(doubleSinAngleABC+doubleSinAngleBAC+doubleSinAngleBCA);
       Vector2 circumCircleCenter = new Vector2(circumCircleCenterX, circumCircleCenterY); 
       return new Circle(circumCircleCenter, circumCircleRadius);
   }

   public static float GetTriangleVerticeAngle(this Triangle _triangle, int vertex)
   {
       float angle = 0;
       switch (vertex)
       {
           case 0:
           {
               angle = Mathf.Acos(Vector2.Dot((_triangle.vertexA - _triangle.vertexB).normalized, (_triangle.vertexA - _triangle.vertexC).normalized));
               break;
           }
           case 1:
           {
               angle = Mathf.Acos(Vector2.Dot((_triangle.vertexB - _triangle.vertexA).normalized, (_triangle.vertexB - _triangle.vertexC).normalized));
               break;
           }
           case 2:
           {
               angle = Mathf.Acos(Vector2.Dot((_triangle.vertexC - _triangle.vertexA).normalized, (_triangle.vertexC - _triangle.vertexB).normalized));
               break;
           }
           default:
           {
               throw new Exception("vertex must be clamp between 0 and 2"); 
               break;
           }
       }
               return angle;
   }
   
   public static Triangle GetTriangleWitchInscribesRect(Rect _rect, float _sizeOfTriangleBaseOffset)
   {
       Vector2 vertexA = new Vector2(_rect.xMin - _sizeOfTriangleBaseOffset, _rect.yMin);
       Vector2 vertexB = new Vector2(_rect.xMax + _sizeOfTriangleBaseOffset, _rect.yMin);
       Vector2 vertexD = new Vector2(_rect.xMin, _rect.yMax);
       float cosBAD = _sizeOfTriangleBaseOffset / Vector2.Distance(vertexA, vertexD);
       float distanceAC = (_sizeOfTriangleBaseOffset+_rect.width/2)/cosBAD;
       Vector2 directionAC = new Vector2(cosBAD, Mathf.Sin(Mathf.Acos(cosBAD)));
       Vector2 vertexC = vertexA + directionAC * distanceAC;
       return new Triangle(vertexA, vertexB, vertexC);
   }
   

}
