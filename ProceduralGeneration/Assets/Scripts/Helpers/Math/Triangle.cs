using System;
using UnityEngine;

[Serializable]
public struct Triangle
{
   public Vector2[] vertices;
   public Triangle(Vector2 _vertexA, Vector2 _vertexB, Vector2 _vertexC)
   {
      vertices = new Vector2[] { _vertexA, _vertexB, _vertexC };
   }
}