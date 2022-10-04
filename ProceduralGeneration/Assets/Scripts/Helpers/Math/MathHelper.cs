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
       float circumCircleCenterX = (_triangle.vertices[0].x * doubleSinAngleBAC + _triangle.vertices[1].x * doubleSinAngleABC +
                                    _triangle.vertices[2].x * doubleSinAngleBCA)/(doubleSinAngleABC+doubleSinAngleBAC+doubleSinAngleBCA);
       float circumCircleCenterY = (_triangle.vertices[0].y * doubleSinAngleBAC + _triangle.vertices[1].y * doubleSinAngleABC +
                                    _triangle.vertices[2].y * doubleSinAngleBCA)/(doubleSinAngleABC+doubleSinAngleBAC+doubleSinAngleBCA);
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
               angle = Mathf.Acos(Vector2.Dot((_triangle.vertices[0] - _triangle.vertices[1]).normalized, (_triangle.vertices[0] - _triangle.vertices[2]).normalized));
               break;
           }
           case 1:
           {
               angle = Mathf.Acos(Vector2.Dot((_triangle.vertices[1] - _triangle.vertices[0]).normalized, (_triangle.vertices[1] - _triangle.vertices[2]).normalized));
               break;
           }
           case 2:
           {
               angle = Mathf.Acos(Vector2.Dot((_triangle.vertices[2] - _triangle.vertices[0]).normalized, (_triangle.vertices[2] -_triangle.vertices[1]).normalized));
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

   public static Segment[] GetSegmentsInTriangles(this Triangle _triangle)
   {
       return new Segment[3]{new (_triangle.vertices[0], _triangle.vertices[1]), new (_triangle.vertices[0], _triangle.vertices[2]), new (_triangle.vertices[1], _triangle.vertices[2])};
   }

   public static bool TriangleHasEdge(this Triangle _triangle, Segment _segment)
   {
    
       
       for (int i = 0; i < _triangle.vertices.Length; i++)
       {
           int sharedVerticesCount = 0;
           if (_triangle.vertices[i] == _segment.PointA || _triangle.vertices[i] == _segment.PointB)
           {
               sharedVerticesCount++;
               if (sharedVerticesCount == 2)
               {
                   return true; 
               }
           }
       }
       return false; 
   }

   public static bool TriangleHasSharedVertices(this Triangle triangleA, Triangle triangleB)
   {
       for (int i = 0; i < triangleA.vertices.Length; i++)
       {
           for (int j = 0; j < triangleB.vertices.Length; j++)
           {
               if (triangleA.vertices[i] == triangleB.vertices[j])
               {
                   return true; 
               }
           }
       }
       return false; 
   }
   

}
