using System;
using UnityEngine;

namespace GeometryHelpers
{
   [Serializable]
   public struct Triangle2DPosition
   {
      
      
      public Vector2[] Vertices
      {
         get => vertices;
      }

      private Vector2[] vertices; 
      
      public Triangle2DPosition(Vector2 _vertexA, Vector2 _vertexB, Vector2 _vertexC)
      {
         vertices = new Vector2[] { _vertexA, _vertexB, _vertexC };
      }
      
      public Triangle2DPosition(Vector2[] _vertices)
      {
         if (_vertices.Length != 3)
            throw new Exception("Vertices count must be equal to 3");
         vertices = _vertices;
      }
   }
}