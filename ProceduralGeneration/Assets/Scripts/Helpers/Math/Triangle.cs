using System;
using UnityEngine;

[Serializable]
public struct Triangle
{
   public Vector2 vertexA;
   public Vector2 vertexB;
   public Vector2 vertexC;
   
   public Triangle(Vector2 _vertexA, Vector2 _vertexB, Vector2 _vertexC)
   {
      vertexA = _vertexA;
      vertexB = _vertexB;
      vertexC = _vertexC;
   }
}