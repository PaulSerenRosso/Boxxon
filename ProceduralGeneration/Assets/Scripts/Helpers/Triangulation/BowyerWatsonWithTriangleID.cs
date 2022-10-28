using System;
using System.Collections.Generic;
using System.Linq;
using GeometryHelpers;
using UnityEngine;
using MeshGenerator;
using Unity.Mathematics;

namespace Triangulation
{
    public class BowyerWatsonWithTriangleID : BowyerWatson
    {
        private Dictionary<Triangle2DPosition, TriangleID> triangleWithTriangleID;
        private TriangleID[] trianglesID;
        private List<Vector2> pointList = new List<Vector2>();

        public BowyerWatsonWithTriangleID(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points) : base(
            _rect, _superTriangleBaseEdgeOffset, _points)
        {
            pointList = _points.ToList();
        }

        public TriangleID[] GetTriangleID()
        {
            if (trianglesID.Length != 0)
            {
                return trianglesID;
            }

            throw new Exception("TrianglesID are not defined please launch Triangulation");
        }
        
        protected override List<Triangle2DPosition> GetTriangleWhichSharedVerticesWithSuperTriangle()
        {
            List<Triangle2DPosition> triangles = new List<Triangle2DPosition>();
            List<TriangleID> newTrianglesID = new List<TriangleID>();
            foreach (var triangle in trianglesWithCircumCircle)
            {
                if (!superTriangle2DPosition.TrianglesHaveOneSharedVertex(triangle.Key))
                {
                    triangles.Add(triangle.Key);
                    int[] indexes = new int[3];
                    for (int i = 0; i < triangle.Key.Vertices.Length; i++)
                    {
                       indexes[i] = pointList.IndexOf(triangle.Key.Vertices[i]);
                    }
               
                    newTrianglesID.Add(new TriangleID(indexes));
                }
            }
            trianglesID = newTrianglesID.ToArray();
            return triangles;
        }
    }
}