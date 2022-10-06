using System;
using UnityEngine;

namespace GeometryHelpers
{
    [Serializable]
    public class Quad
    {
        public Vector2[] Vertices;

        public Quad(Vector2 _vertexA, Vector2 _vertexB, Vector2 _vertexC, Vector2 _vertexD)
        {
            Vertices = new[] { _vertexA, _vertexB, _vertexC, _vertexD };
        }

    }
}