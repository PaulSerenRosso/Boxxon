using System;
using UnityEngine;

namespace GeometryHelpers
{
   [Serializable]
   public struct Triangle
   {
      public Vector2[] Vertices;

      public Triangle(Vector2 _vertexA, Vector2 _vertexB, Vector2 _vertexC)
      {
         Vertices = new Vector2[] { _vertexA, _vertexB, _vertexC };
      }
   }
}