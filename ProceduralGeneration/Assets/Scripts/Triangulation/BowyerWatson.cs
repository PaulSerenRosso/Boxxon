using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;

namespace Triangulation
{
    public class BowyerWatson
    {
        private Rect rect;

        private float superTriangleBaseEdgeOffset;

        Triangle superTriangle;
        Dictionary<Triangle, Circle> trianglesWithCircumCircle = new Dictionary<Triangle, Circle>();
        Dictionary<Triangle, Segment[]> trianglesWithEdges = new Dictionary<Triangle, Segment[]>();
        private Vector2[] points;

        public BowyerWatson(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points)
        {
            rect = _rect;
            superTriangleBaseEdgeOffset = _superTriangleBaseEdgeOffset;
            points = _points;
            superTriangle = GeometryHelper.GetTriangleWitchInscribesRect(rect, superTriangleBaseEdgeOffset);
        }

        public Triangle[] Triangulate()
        {
            trianglesWithCircumCircle.Add(superTriangle, superTriangle.GetTriangleCircumCircle());
            for (int i = 0; i < points.Length; i++)
            {
                List<Triangle> trianglesChoosen = new List<Triangle>();
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
                    Triangle newTriangle = new Triangle(points[i], polygone[j].Points[0], polygone[j].Points[1]);
                    trianglesWithCircumCircle.Add(newTriangle, newTriangle.GetTriangleCircumCircle());
                }
            }

            List<Triangle> triangles = new List<Triangle>();
            foreach (var triangle in trianglesWithCircumCircle)
            {
                if (!superTriangle.TrianglesHaveOneSharedVertex(triangle.Key))
                {
                    triangles.Add(triangle.Key);
                }
            }

            // delete tous les triangles qui partages des vertices avec au moins deux vertices avec le super triangles 
            return triangles.ToArray();
        }

        Segment[] GetTriangleEdges(Triangle triangle)
        {
            if (!trianglesWithEdges.ContainsKey(triangle))
            {
                trianglesWithEdges.Add(triangle, triangle.GetSegmentsInTriangles());
            }

            return trianglesWithEdges[triangle];
        }
    }
}