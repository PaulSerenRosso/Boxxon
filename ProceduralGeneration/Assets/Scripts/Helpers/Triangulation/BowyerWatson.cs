using System;
using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;

namespace Triangulation
{
    public class BowyerWatson
    {
        private Rect rect;

        private float superTriangleBaseEdgeOffset;

        public Triangle2DPosition superTriangle2DPosition;
        

        protected Dictionary<Triangle2DPosition, Circle> trianglesWithCircumCircle =
            new Dictionary<Triangle2DPosition, Circle>();

        Dictionary<Triangle2DPosition, Segment[]> trianglesWithEdges = new Dictionary<Triangle2DPosition, Segment[]>();
        protected private Vector2[] points;

        public BowyerWatson(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points)
        {
            rect = _rect;
            superTriangleBaseEdgeOffset = _superTriangleBaseEdgeOffset;
            points = _points;
            superTriangle2DPosition = GeometryHelper.GetTriangleWitchInscribesRect(rect, superTriangleBaseEdgeOffset);
        }

        public Vector2[] GetPoints()
        {
            if (points != null)
            {
                return points;
            }

            throw new Exception("Points are not defined please set Points First");
        }

        public Triangle2DPosition[] Triangulate()
        {
            trianglesWithCircumCircle.Add(superTriangle2DPosition, superTriangle2DPosition.GetTriangleCircumCircle());
            for (int i = 0; i < points.Length; i++)
            {
                var trianglesChoosen = ChooseTriangles(i);

                var polygon = CreatePolygon(trianglesChoosen);
                CreateNewTriangles(polygon, i);
                RemoveChoosenTriangle(trianglesChoosen);
            }

            var triangles = GetTriangleWhichSharedVerticesWithSuperTriangle();

            return triangles.ToArray();
        }

        Segment[] GetTriangleEdges(Triangle2DPosition _triangle2DPosition)
        {
            trianglesWithEdges.Add(_triangle2DPosition, _triangle2DPosition.GetSegmentsInTriangles());

            return trianglesWithEdges[_triangle2DPosition];
        }

        private List<Triangle2DPosition> ChooseTriangles(int _i)
        {
            List<Triangle2DPosition> trianglesChoosen = new List<Triangle2DPosition>();
            foreach (var triangleWithCircumCircle in trianglesWithCircumCircle)
            {
                if ((points[_i] - triangleWithCircumCircle.Value.center).sqrMagnitude <=
                    triangleWithCircumCircle.Value.radius * triangleWithCircumCircle.Value.radius)
                {
                    trianglesChoosen.Add(triangleWithCircumCircle.Key);
                }
            }

            return trianglesChoosen;
        }

        private List<Segment> CreatePolygon(List<Triangle2DPosition> _trianglesChoosen)
        {
            List<Segment> polygon = new List<Segment>();
            for (int j = 0; j < _trianglesChoosen.Count; j++)
            {
                Segment[] triangleEdges = GetTriangleEdges(_trianglesChoosen[j]);
                for (int k = 0; k < triangleEdges.Length; k++)
                {
                    bool isValid = true;
                    for (int i = 0; i < polygon.Count; i++)
                    {
                        int sharedVertex = 0;
                        for (int l = 0; l < triangleEdges[k].Points.Length; l++)
                        {
                            if (triangleEdges[k].Points[l] == polygon[i].Points[0] ||
                                triangleEdges[k].Points[l] == polygon[i].Points[1])
                            {
                                sharedVertex++;
                                if (sharedVertex == 2)
                                {
                                    polygon.RemoveAt(i);
                                    isValid = false;
                                }
                            }
                        }
                    }
                    if (isValid)
                    {
                        polygon.Add(triangleEdges[k]);
                    }
                }
            }

            return polygon;
        }

        private void RemoveChoosenTriangle(List<Triangle2DPosition> _trianglesChoosen)
        {
            for (int j = _trianglesChoosen.Count - 1; j >= 0; j--)
            {
                trianglesWithEdges.Remove(_trianglesChoosen[j]);
                trianglesWithCircumCircle.Remove(_trianglesChoosen[j]);
            }
        }

        protected void CreateNewTriangles(List<Segment> _polygone, int i)
        {
            for (int j = 0; j < _polygone.Count; j++)
            {
                Triangle2DPosition newTriangle2DPosition =
                    new Triangle2DPosition(points[i], _polygone[j].Points[0], _polygone[j].Points[1]);
                trianglesWithCircumCircle.Add(newTriangle2DPosition, newTriangle2DPosition.GetTriangleCircumCircle());
            }
        }

        protected virtual List<Triangle2DPosition> GetTriangleWhichSharedVerticesWithSuperTriangle()
        {
            List<Triangle2DPosition> triangles = new List<Triangle2DPosition>();
            foreach (var triangle in trianglesWithCircumCircle)
            {
                if (!superTriangle2DPosition.TrianglesHaveOneSharedVertex(triangle.Key))
                {
                    triangles.Add(triangle.Key);
                }
            }

            return triangles;
        }
    }
}