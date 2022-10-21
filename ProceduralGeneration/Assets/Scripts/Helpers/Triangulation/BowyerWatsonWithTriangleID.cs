using System;
using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;
using MeshGenerator;
namespace Triangulation
{
    public class BowyerWatsonWithTriangleID : BowyerWatson
    {
        private Dictionary<Triangle2DPosition, TriangleID> triangleWithTriangleID;
        private TriangleID[] trianglesID; 
        public BowyerWatsonWithTriangleID(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points) : base(_rect, _superTriangleBaseEdgeOffset, _points)
        {
        }

        public TriangleID[] GetTriangleID()
        {
            if (trianglesID != null)
            {
                return trianglesID;
            }

            throw new Exception("TrianglesID are not defined please launch tTriangulation");
        }

        protected override void CreateNewTriangles(List<Segment> _polygone, int i)
        {
            for (int j = 0; j < _polygone.Count; j++)
            {
                Triangle2DPosition newTriangle2DPosition =
                    new Triangle2DPosition(points[i], _polygone[j].Points[0], _polygone[j].Points[1]);

                trianglesWithCircumCircle.Add(newTriangle2DPosition, newTriangle2DPosition.GetTriangleCircumCircle());
            }
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
                    newTrianglesID.Add(triangleWithTriangleID[triangle.Key]);
                }
            }
            trianglesID = newTrianglesID.ToArray();
            return triangles;
        }
    }
}