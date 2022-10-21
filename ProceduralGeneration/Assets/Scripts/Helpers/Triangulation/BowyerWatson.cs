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

       protected Triangle2DPosition superTriangle2DPosition;
      protected  Dictionary<Triangle2DPosition, Circle> trianglesWithCircumCircle = new Dictionary<Triangle2DPosition, Circle>();
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

                var polygone = CreatePolygone(trianglesChoosen);
                RemoveChoosenTriangle(trianglesChoosen);

                CreateNewTriangles(polygone, i);
            }

            var triangles = GetTriangleWhichSharedVerticesWithSuperTriangle();

            // delete tous les triangles qui partages des vertices avec au moins deux vertices avec le super triangles 
            return triangles.ToArray();
        }
        Segment[] GetTriangleEdges(Triangle2DPosition _triangle2DPosition)
        {
            if (!trianglesWithEdges.ContainsKey(_triangle2DPosition))
            {
                trianglesWithEdges.Add(_triangle2DPosition, _triangle2DPosition.GetSegmentsInTriangles());
            }

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

        private List<Segment> CreatePolygone(List<Triangle2DPosition> _trianglesChoosen)
        {
            List<Segment> polygone = new List<Segment>();

            for (int j = 0; j < _trianglesChoosen.Count; j++)
            {
                Segment[] triangleEdges = GetTriangleEdges(_trianglesChoosen[j]);
                for (int k = 0; k < triangleEdges.Length; k++)
                {
                    for (int l = 0; l < _trianglesChoosen.Count; l++)
                    {
                        if (j != l && _trianglesChoosen[l].TriangleHasEdge(triangleEdges[k]))
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

            return polygone;
        }
        private void RemoveChoosenTriangle(List<Triangle2DPosition> _trianglesChoosen)
        {
            for (int j = _trianglesChoosen.Count - 1; j >= 0; j--)
            {
                trianglesWithEdges.Remove(_trianglesChoosen[j]);
            }
        }
        protected virtual void CreateNewTriangles(List<Segment> _polygone, int i)
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