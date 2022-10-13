using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;

namespace Triangulation
{
    public class BowyerWatson
    {
        private Rect rect;

        private float superTriangleBaseEdgeOffset;

        Triangle2DPosition superTriangle2DPosition;
        Dictionary<Triangle2DPosition, Circle> trianglesWithCircumCircle = new Dictionary<Triangle2DPosition, Circle>();
        Dictionary<Triangle2DPosition, Segment[]> trianglesWithEdges = new Dictionary<Triangle2DPosition, Segment[]>();
        private Vector2[] points;

        public BowyerWatson(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points)
        {
            rect = _rect;
            superTriangleBaseEdgeOffset = _superTriangleBaseEdgeOffset;
            points = _points;
            superTriangle2DPosition = GeometryHelper.GetTriangleWitchInscribesRect(rect, superTriangleBaseEdgeOffset);
        }

        public Triangle2DPosition[] Triangulate()
        {
            trianglesWithCircumCircle.Add(superTriangle2DPosition, superTriangle2DPosition.GetTriangleCircumCircle());
            for (int i = 0; i < points.Length; i++)
            {
                List<Triangle2DPosition> trianglesChoosen = new List<Triangle2DPosition>();
                foreach (var triangleWithCircumCircle in trianglesWithCircumCircle)
                {
                    if ((points[i] - triangleWithCircumCircle.Value.center).sqrMagnitude <=
                        triangleWithCircumCircle.Value.radius * triangleWithCircumCircle.Value.radius)
                    {
                        trianglesChoosen.Add(triangleWithCircumCircle.Key);
                    }
                }

                List<Segment> polygone = new List<Segment>();

                for (int j = 0; j < trianglesChoosen.Count; j++)
                {
                    Segment[] triangleEdges = GetTriangleEdges(trianglesChoosen[j]);
                    for (int k = 0; k < triangleEdges.Length; k++)
                    {
                        for (int l = 0; l < trianglesChoosen.Count; l++)
                        {
                            if (j != l && trianglesChoosen[l].TriangleHasEdge(triangleEdges[k]))
                            {
                                if (!polygone.Contains(triangleEdges[k]))
                                {
                                    polygone.Add(triangleEdges[k]);
                                }
                                else
                                {
                                    polygone.Remove(triangleEdges[k]);
                                }
                            }
                        }
                    }
                }

                for (int j = trianglesChoosen.Count - 1; j >= 0; j--)
                {
                    trianglesWithEdges.Remove(trianglesChoosen[j]);
                }

                for (int j = 0; j < polygone.Count; j++)
                {
                    Triangle2DPosition newTriangle2DPosition = new Triangle2DPosition(points[i], polygone[j].Points[0], polygone[j].Points[1]);
                    trianglesWithCircumCircle.Add(newTriangle2DPosition, newTriangle2DPosition.GetTriangleCircumCircle());
                }
            }

            List<Triangle2DPosition> triangles = new List<Triangle2DPosition>();
            foreach (var triangle in trianglesWithCircumCircle)
            {
                if (!superTriangle2DPosition.TrianglesHaveOneSharedVertex(triangle.Key))
                {
                    triangles.Add(triangle.Key);
                }
            }

            // delete tous les triangles qui partages des vertices avec au moins deux vertices avec le super triangles 
            return triangles.ToArray();
        }

        Segment[] GetTriangleEdges(Triangle2DPosition triangle2DPosition)
        {
            if (!trianglesWithEdges.ContainsKey(triangle2DPosition))
            {
                trianglesWithEdges.Add(triangle2DPosition, triangle2DPosition.GetSegmentsInTriangles());
            }

            return trianglesWithEdges[triangle2DPosition];
        }
    }
}