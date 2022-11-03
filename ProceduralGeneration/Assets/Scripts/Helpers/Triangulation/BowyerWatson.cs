using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeometryHelpers;
using UnityEngine;

namespace Triangulation
{
    public class BowyerWatson
    {
        private Rect rect;

        private float superTriangleBaseEdgeOffset;

        public Triangle2DPosition superTriangle2DPosition;
        // aléatoire retirer les angles trop optu

        protected float maxAngle = 0;

        public Dictionary<Triangle2DPosition, Circle> trianglesWithCircumCircle =
            new Dictionary<Triangle2DPosition, Circle>();

        protected private Vector2[] points;

        public BowyerWatson(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points, float _maxAngle)
        {
            rect = _rect;
            maxAngle = _maxAngle;
            superTriangleBaseEdgeOffset = _superTriangleBaseEdgeOffset;
            points = _points;
            superTriangle2DPosition = GeometryHelper.GetTriangleWitchInscribesRect(_rect, superTriangleBaseEdgeOffset);
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

                var triangleWhichContainCurrentPoint = GetTriangleWithCurrentPoint(trianglesChoosen, i);
                var polygon = CreatePolygon(trianglesChoosen, triangleWhichContainCurrentPoint);
                RemoveChoosenTriangle(trianglesChoosen);
                CreateNewTriangles(polygon, i);
            }

            var triangles = FilterTriangles();

            return triangles.ToArray();
        }

        Segment[] GetTriangleEdges(Triangle2DPosition _triangle2DPosition)
        {
            return _triangle2DPosition.GetSegmentsInTriangles();
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

        private Triangle2DPosition GetTriangleWithCurrentPoint(List<Triangle2DPosition> _trianglesChoosen, int _i)
        {
            for (int i = 0; i < _trianglesChoosen.Count; i++)
            {
                if (_trianglesChoosen[i].CheckIfPointIsInTriangle(points[_i]))
                {
                     return _trianglesChoosen[i];
                }
            }
            throw new Exception("Triangles choosen List doesn't contain the triangle which contain the current point");
        }


        private List<Segment> CreatePolygon(List<Triangle2DPosition> _trianglesChoosen, Triangle2DPosition _triangleWhichContainCurrentPoint)
        {
            var polygon = CreatePolygonWithNoOneDuplication(_trianglesChoosen);
            
           int[] sharedVerticesCount = new int[polygon.Count];
            
            for (int i = polygon.Count-1; i > -1; i--)
            {
                for (int j = polygon.Count-1; j > -1; j--)
                {
                  sharedVerticesCount[i] += polygon[i].GetSharedVertices(polygon[j]);
                }

                if (sharedVerticesCount[i] != 2)
                {
                    polygon.RemoveAt(i);
                }
            }

            


            // check si les edges ont deux fois les memes
            
            // check si un points puis un points 
            return polygon;
        }

        private List<Segment> CreatePolygonWithNoOneDuplication(List<Triangle2DPosition> _trianglesChoosen)
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
                                    break;
                                }
                            }
                        }

                        if (!isValid)
                        {
                            break;
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
            for (int i = 0; i < _trianglesChoosen.Count; i++)
            {
                trianglesWithCircumCircle.Remove(_trianglesChoosen[i]);
            }
        }

        protected void CreateNewTriangles(List<Segment> _polygone, int i)
        {
            
            for (int j = 0; j < _polygone.Count; j++)
            {
                Triangle2DPosition newTriangle2DPosition =
                    new Triangle2DPosition(points[i], _polygone[j].Points[0], _polygone[j].Points[1]);
                var triangleCircumCircle = newTriangle2DPosition.GetTriangleCircumCircle();
              
                trianglesWithCircumCircle.Add(newTriangle2DPosition, triangleCircumCircle);
                
            }
        }

        protected virtual List<Triangle2DPosition> FilterTriangles()
        {
            List<Triangle2DPosition> triangles = new List<Triangle2DPosition>();
            foreach (var triangle in trianglesWithCircumCircle)
            {
                if (!superTriangle2DPosition.TrianglesHaveOneSharedVertex(triangle.Key))
                {
                    Vector2[] vertices = triangle.Key.Vertices;
                    bool hasTooLargeAngle = false;
                    hasTooLargeAngle = CheckAngle(vertices, maxAngle);

                    if (!hasTooLargeAngle)
                    {
                        triangles.Add(triangle.Key);
                    }
                }
            }

            return triangles;
        }

        protected bool CheckAngle(Vector2[] _vertices, float _maxAngle)
        {
            bool hasTooLargeAngle = false;
            Vector2[] edgesVector = new[]
                { _vertices[1] - _vertices[0], _vertices[2] - _vertices[1], _vertices[0] - _vertices[2] };
            float[] verticesAngle = new float[3];
            verticesAngle[0] = Vector2.Angle(-edgesVector[0], edgesVector[1]);
            verticesAngle[1] = Vector2.Angle(-edgesVector[1], edgesVector[2]);
            verticesAngle[2] = Vector2.Angle(edgesVector[0], -edgesVector[2]);
            for (int i = 0; i < verticesAngle.Length; i++)
            {
                if (verticesAngle[i] > _maxAngle)
                {
                    hasTooLargeAngle = true;
                    break;
                }
            }

            return hasTooLargeAngle;
        }
    }
}