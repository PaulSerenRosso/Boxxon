using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AlgebraHelpers;
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

        protected float maxAngleForFilteringFinalTriangles = 0;

        protected Dictionary<Triangle2DPosition, Circle> trianglesWithCircumCircle =
            new Dictionary<Triangle2DPosition, Circle>();



        protected private Vector2[] points;

        public BowyerWatson(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points,
            float _maxAngleForFilteringFinalTriangles)
        {

            rect = new Rect(_rect.size*2 / 2, _rect.size*2);
            
            
            maxAngleForFilteringFinalTriangles = _maxAngleForFilteringFinalTriangles;
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
                var filteredTrianglesChoosen =
                    FilteredTrianglesChoosen(trianglesChoosen, triangleWhichContainCurrentPoint, i);
                var polygon = CreatePolygon(filteredTrianglesChoosen);
                CreateNewTriangles(polygon, i);
                RemoveChoosenTriangle(filteredTrianglesChoosen);
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
            float[] subtractsOfTriangleAreaAndSubTrianglesAreaComposedWithPoint = new float[_trianglesChoosen.Count];
            int minSubtractOfTriangleAreaAndSubTrianglesAreaComposedWithPointIndex = 0; 
            for (int i = 0; i < _trianglesChoosen.Count; i++)
            {
                subtractsOfTriangleAreaAndSubTrianglesAreaComposedWithPoint[i] = _trianglesChoosen[i]
                    .GetSubtractOfTriangleAreaAndSubTrianglesAreaComposedWithPoint(points[_i]);
            }

            minSubtractOfTriangleAreaAndSubTrianglesAreaComposedWithPointIndex = 0;
            for (int j = 1; j < subtractsOfTriangleAreaAndSubTrianglesAreaComposedWithPoint.Length; j++)
                {
                    if (subtractsOfTriangleAreaAndSubTrianglesAreaComposedWithPoint
                            [minSubtractOfTriangleAreaAndSubTrianglesAreaComposedWithPointIndex] >
                        subtractsOfTriangleAreaAndSubTrianglesAreaComposedWithPoint[j])
                    {
                        minSubtractOfTriangleAreaAndSubTrianglesAreaComposedWithPointIndex =
                         j;
                    }
                }
            return _trianglesChoosen[minSubtractOfTriangleAreaAndSubTrianglesAreaComposedWithPointIndex];
        }

                       private List<Triangle2DPosition> FilteredTrianglesChoosen(List<Triangle2DPosition> _trianglesChoosen,
            Triangle2DPosition _triangleWhichContainCurrentPoint, int _i)
        {
            Triangle2DPosition currentTriangle = _triangleWhichContainCurrentPoint;
            List<Triangle2DPosition> filteredTriangleChoosen = new List<Triangle2DPosition>();
            List<Triangle2DPosition> trianglesWhichNeedToCheckNeighboursTriangles = new List<Triangle2DPosition>();
            bool needNewIteration = true;
            filteredTriangleChoosen.Add(currentTriangle);
            trianglesWhichNeedToCheckNeighboursTriangles.Add(currentTriangle);
            _trianglesChoosen.Remove(currentTriangle);
            while (needNewIteration)
            {
                for (int i = _trianglesChoosen.Count - 1; i > -1; i--)
                {
                    List<Vector2> sharedVertices =
                        currentTriangle.GetSharedVertices(_trianglesChoosen[i]);
                    
                    if (sharedVertices.Count == 2)
                    {
                     ;
                    float[] maxDirectionAngles = new float[2];
                    bool maxAngleMustBeReverse = false;
                        for (int j = 0; j < 2; j++)
                        {
                           Vector2 maxDirection = (sharedVertices[j] - points[_i]).normalized;
                           maxDirectionAngles[j] = Mathf.Atan2(maxDirection.y, maxDirection.x);
                           if (maxDirectionAngles[j] < 0)
                           {
                               maxDirectionAngles[j] = Mathf.PI * 2 + maxDirectionAngles[j];
                           }
                           
                        }

                        float maxAngle = maxDirectionAngles[0] - maxDirectionAngles[1];
                    
                        if (maxAngle > 0)
                        {
                            (maxDirectionAngles[0], maxDirectionAngles[1]) = (maxDirectionAngles[1], maxDirectionAngles[0]);
                           
                        }
                        else
                        {
                            maxAngle = maxDirectionAngles[1] - maxDirectionAngles[0];
                        }
                        
                        if  (maxAngle.IsClamp(Mathf.PI-0.01f,Mathf.PI+0.01f))
                        {
                            Vector2 oppositeVertexOfCurrentTriangle = Vector2.zero;
                            for (int j = 0; j < currentTriangle.Vertices.Length; j++)
                            {
                                if (currentTriangle.Vertices[j] != sharedVertices[0]
                                    && currentTriangle.Vertices[j] != sharedVertices[1])
                                {
                                    oppositeVertexOfCurrentTriangle = currentTriangle.Vertices[j];
                                    break;
                                }
                                
                            }
                      
                            Vector2 directionOppositeAngleOfCurrentTriangleToCurrentPoint = (oppositeVertexOfCurrentTriangle - points[_i]).normalized;
                            float currentDirectionOppositeAngleOfCurrentTriangle = Mathf.Atan2( directionOppositeAngleOfCurrentTriangleToCurrentPoint.y,
                                directionOppositeAngleOfCurrentTriangleToCurrentPoint.x);
                            if (currentDirectionOppositeAngleOfCurrentTriangle.IsClamp(maxDirectionAngles[0],
                                maxDirectionAngles[1]))
                            {
                                maxAngle = 2 * Mathf.PI - maxAngle;
                                maxAngleMustBeReverse = true;
                            }
                        }
                       else if (maxAngle >= Mathf.PI)
                        {
                            maxAngle = 2 * Mathf.PI - maxAngle;
                            maxAngleMustBeReverse = true;
                        }
                        
                        
                        Vector2 oppositeVertexOfTriangleWhichBeInCheck = Vector2.zero;
                        for (int j = 0; j < _trianglesChoosen[i].Vertices.Length; j++)
                        {
                            if (_trianglesChoosen[i].Vertices[j] != sharedVertices[0]
                                && _trianglesChoosen[i].Vertices[j] != sharedVertices[1])
                            {
                                oppositeVertexOfTriangleWhichBeInCheck = _trianglesChoosen[i].Vertices[j];
                                break;
                            }
                        }
                        Vector2 directionOppositeAngleOfTriangleWhichBeInCheckToCurrentPoint = (oppositeVertexOfTriangleWhichBeInCheck - points[_i]).normalized;
                      float currentDirectionAngleOfTriangleWhichBeInCheckToCurrentPoint = Mathf.Atan2(directionOppositeAngleOfTriangleWhichBeInCheckToCurrentPoint.y,
                            directionOppositeAngleOfTriangleWhichBeInCheckToCurrentPoint.x);
                      if (currentDirectionAngleOfTriangleWhichBeInCheckToCurrentPoint < 0)
                      {
                          currentDirectionAngleOfTriangleWhichBeInCheckToCurrentPoint = Mathf.PI * 2 + currentDirectionAngleOfTriangleWhichBeInCheckToCurrentPoint;
                      }
                    
                     bool currentDirectionAngleIsValided = false;
                      if (maxAngleMustBeReverse)
                      {
                          if (currentDirectionAngleOfTriangleWhichBeInCheckToCurrentPoint.IsClamp(0, maxDirectionAngles[0]) ||
                              currentDirectionAngleOfTriangleWhichBeInCheckToCurrentPoint.IsClamp(maxDirectionAngles[1], Mathf.PI * 2))
                          {
                              currentDirectionAngleIsValided = true;
                          }
                      }
                      else
                      {
                          if (currentDirectionAngleOfTriangleWhichBeInCheckToCurrentPoint.IsClamp(maxDirectionAngles[0], maxDirectionAngles[1]))
                          {
                              currentDirectionAngleIsValided = true;
                                  
                          }
                      }
                        
                        if (currentDirectionAngleIsValided)
                        {
                            filteredTriangleChoosen.Add(_trianglesChoosen[i]);
                            trianglesWhichNeedToCheckNeighboursTriangles.Add(_trianglesChoosen[i]);
                            _trianglesChoosen.RemoveAt(i);
                        }
                    }
                }

                trianglesWhichNeedToCheckNeighboursTriangles.Remove(currentTriangle);
                if (trianglesWhichNeedToCheckNeighboursTriangles.Count != 0)
                {
                    needNewIteration = true;
                    currentTriangle = trianglesWhichNeedToCheckNeighboursTriangles[0];
                }
                else
                {
                    needNewIteration = false;
                }
            }


            return filteredTriangleChoosen;
        }

        private List<Segment> CreatePolygon(List<Triangle2DPosition> _trianglesChoosen)
        {
            var polygonWithNoOneDuplication = CreatePolygonWithNoOneDuplication(_trianglesChoosen);
            return polygonWithNoOneDuplication;
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
                    hasTooLargeAngle = CheckAngle(vertices, maxAngleForFilteringFinalTriangles);

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