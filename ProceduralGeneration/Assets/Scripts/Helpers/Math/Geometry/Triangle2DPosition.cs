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
      [SerializeField]
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
      public static bool operator == (Triangle2DPosition a, Triangle2DPosition b)
      {
         int sharedVertices = 0;
         for (int i = 0; i < a.vertices.Length; i++)
         {
            for (int j = 0; j < b.vertices.Length; j++)
            {
               if (a.vertices[i] == b.vertices[j])
               {
                  sharedVertices++;
               }
            }
         }

         if (sharedVertices == 3)
         {
            return true;
         }

         return false;
      }

      public static bool operator !=(Triangle2DPosition a, Triangle2DPosition b)
      {
         return !(a == b);
      }
   }
}