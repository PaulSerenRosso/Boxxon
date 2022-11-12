using System;
using UnityEngine;

namespace GeometryHelpers
{
    [Serializable]
    public struct Quad2DPosition
    {
        public Vector2[] Vertices
        {
            get => vertices;
        }

        [SerializeField] Vector2[] vertices;

        public Quad2DPosition(Vector2 _vertexA, Vector2 _vertexB, Vector2 _vertexC, Vector2 _vertexD)
        {
            vertices = new[] {_vertexA, _vertexB, _vertexC, _vertexD};
        }

        public Quad2DPosition(Vector2[] _vertices)
        {
            if (_vertices.Length != 4)
                throw new Exception("Vertices count must be equal to 4");

            vertices = _vertices;
        }
    }
}