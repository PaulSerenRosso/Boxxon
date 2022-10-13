using System;
using System.Collections.Generic;
using UnityEngine;


namespace GeometryHelpers
{
    public static class GeometryHelper
    {
        public static Circle GetTriangleCircumCircle(this Triangle2DPosition triangle2DPosition)
        {
            float angleBAC = triangle2DPosition.GetTriangleVerticeAngle(0);
            float angleABC = triangle2DPosition.GetTriangleVerticeAngle(1);
            float angleBCA = Mathf.PI - (angleABC + angleBAC);
            float doubleSinAngleBAC = Mathf.Sin(angleBAC) * 2;
            float doubleSinAngleABC = Mathf.Sin(angleABC) * 2;
            float doubleSinAngleBCA = Mathf.Sin(angleBCA) * 2;
            float circumCircleRadius = angleBAC / doubleSinAngleBAC;
            float circumCircleCenterX = (triangle2DPosition.Vertices[0].x * doubleSinAngleBAC +
                                         triangle2DPosition.Vertices[1].x * doubleSinAngleABC +
                                         triangle2DPosition.Vertices[2].x * doubleSinAngleBCA) /
                                        (doubleSinAngleABC + doubleSinAngleBAC + doubleSinAngleBCA);
            float circumCircleCenterY = (triangle2DPosition.Vertices[0].y * doubleSinAngleBAC +
                                         triangle2DPosition.Vertices[1].y * doubleSinAngleABC +
                                         triangle2DPosition.Vertices[2].y * doubleSinAngleBCA) /
                                        (doubleSinAngleABC + doubleSinAngleBAC + doubleSinAngleBCA);
            Vector2 circumCircleCenter = new Vector2(circumCircleCenterX, circumCircleCenterY);
            return new Circle(circumCircleCenter, circumCircleRadius);
        }

        public static float GetTriangleVerticeAngle(this Triangle2DPosition triangle2DPosition, int vertex)
        {
            float angle = 0;
            switch (vertex)
            {
                case 0:
                {
                    angle = Mathf.Acos(Vector2.Dot((triangle2DPosition.Vertices[0] - triangle2DPosition.Vertices[1]).normalized,
                        (triangle2DPosition.Vertices[0] - triangle2DPosition.Vertices[2]).normalized));
                    break;
                }
                case 1:
                {
                    angle = Mathf.Acos(Vector2.Dot((triangle2DPosition.Vertices[1] - triangle2DPosition.Vertices[0]).normalized,
                        (triangle2DPosition.Vertices[1] - triangle2DPosition.Vertices[2]).normalized));
                    break;
                }
                case 2:
                {
                    angle = Mathf.Acos(Vector2.Dot((triangle2DPosition.Vertices[2] - triangle2DPosition.Vertices[0]).normalized,
                        (triangle2DPosition.Vertices[2] - triangle2DPosition.Vertices[1]).normalized));
                    break;
                }
                default:
                {
                    throw new Exception("vertex must be clamp between 0 and 2");
                    break;
                }
            }

            return angle;
        }

        public static Triangle2DPosition GetTriangleWitchInscribesRect(Rect _rect, float _sizeOfTriangleBaseOffset)
        {
            Vector2 vertexA = new Vector2(_rect.xMin - _sizeOfTriangleBaseOffset, _rect.yMin);
            Vector2 vertexB = new Vector2(_rect.xMax + _sizeOfTriangleBaseOffset, _rect.yMin);
            Vector2 vertexD = new Vector2(_rect.xMin, _rect.yMax);
            float cosBAD = _sizeOfTriangleBaseOffset / Vector2.Distance(vertexA, vertexD);
            float distanceAC = (_sizeOfTriangleBaseOffset + _rect.width / 2) / cosBAD;
            Vector2 directionAC = new Vector2(cosBAD, Mathf.Sin(Mathf.Acos(cosBAD)));
            Vector2 vertexC = vertexA + directionAC * distanceAC;
            return new Triangle2DPosition(vertexA, vertexB, vertexC);
        }

        public static Segment[] GetSegmentsInTriangles(this Triangle2DPosition triangle2DPosition)
        {
            return new Segment[3]
            {
                new(triangle2DPosition.Vertices[0], triangle2DPosition.Vertices[1]), new(triangle2DPosition.Vertices[0], triangle2DPosition.Vertices[2]),
                new(triangle2DPosition.Vertices[1], triangle2DPosition.Vertices[2])
            };
        }

        public static bool TriangleHasEdge(this Triangle2DPosition triangle2DPosition, Segment _segment)
        {
            for (int j = 0; j < _segment.Points.Length; j++)
            {
                for (int i = 0; i < triangle2DPosition.Vertices.Length; i++)
                {
                    int sharedVerticesCount = 0;
                    if (triangle2DPosition.Vertices[i] == _segment.Points[j])
                    {
                        sharedVerticesCount++;
                        if (sharedVerticesCount == 2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool TrianglesHaveOneSharedVertex(this Triangle2DPosition triangle2DPositionA, Triangle2DPosition triangle2DPositionB)
        {
            for (int i = 0; i < triangle2DPositionA.Vertices.Length; i++)
            {
                for (int j = 0; j < triangle2DPositionB.Vertices.Length; j++)
                {
                    if (triangle2DPositionA.Vertices[i] == triangle2DPositionB.Vertices[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static Vector2 GetCommunVertexOfTriangles(this Triangle2DPosition triangle2DPositionA, Triangle2DPosition triangle2DPositionB)
        {
            for (int i = 0; i < triangle2DPositionA.Vertices.Length; i++)
            {
                for (int j = 0; j < triangle2DPositionB.Vertices.Length; j++)
                {
                    if (triangle2DPositionA.Vertices[i] == triangle2DPositionB.Vertices[j])
                    {
                        return triangle2DPositionA.Vertices[i];
                    }
                }
            }

            throw new Exception("Triangles don't have one same vertex");
        }

        public static Quad CreateQuadWithTwoTriangle(this Triangle2DPosition triangle2DPositionA, Triangle2DPosition triangle2DPositionB, Segment _communEdge)
        {
            Triangle2DPosition[] triangles = new[] { triangle2DPositionA, triangle2DPositionB };
            //check pour 
            List<Vector2> unsharedPoints = new List<Vector2>();
            for (int i = 0; i < _communEdge.Points.Length; i++)
            {
                for (int j = 0; j < triangles.Length; j++)
                {
                    for (int k = 0; k < triangles[j].Vertices.Length; k++)
                    {
                        if (triangles[j].Vertices[k] != _communEdge.Points[i])
                        {
                            unsharedPoints.Add(triangles[j].Vertices[k]);
                        }
                    }
                }
            }

            if (unsharedPoints.Count != 2)
                throw new Exception("Triangles have not one same edge or the current segment is not the good one");
            return new Quad(_communEdge.Points[0], _communEdge.Points[1], unsharedPoints[0], unsharedPoints[1]);
        }

        public static bool TrianglesHaveOneSameEdge(this Triangle2DPosition triangle2DPositionA, Triangle2DPosition triangle2DPositionB)
        {
            int verticesCount = 0;
            for (int i = 0; i < triangle2DPositionA.Vertices.Length; i++)
            {
                for (int j = 0; j < triangle2DPositionB.Vertices.Length; j++)
                {
                    if (triangle2DPositionA.Vertices[i] == triangle2DPositionB.Vertices[j])
                    {
                        verticesCount++;
                    }
                }
            }

            return verticesCount == 2;
        }

        public static Segment GetSameEdgeOfTriangles(this Triangle2DPosition triangle2DPositionA, Triangle2DPosition triangle2DPositionB)
        {
            List<Vector2> sameVertices = new List<Vector2>();
            for (int i = 0; i < triangle2DPositionA.Vertices.Length; i++)
            {
                for (int j = 0; j < triangle2DPositionB.Vertices.Length; j++)
                {
                    if (triangle2DPositionA.Vertices[i] == triangle2DPositionB.Vertices[j])
                    {
                        sameVertices.Add(triangle2DPositionA.Vertices[i]);
                    }
                }
            }

            if (sameVertices.Count == 2)
            {
                return new Segment(sameVertices[0], sameVertices[1]);
            }

            throw new Exception("Triangles don't have one same edge");
        }

        public static Vector2 GetPolygonCenter(Vector2[] _vertices)
        {
            Vector2 center = Vector2.zero;
            for (int i = 0; i < _vertices.Length; i++)
            {
                center += _vertices[i];
            }
            center /= _vertices.Length;
            return center;
        }

        public static List<Vector2> GetMidEdgePoints(this Vector2[] _vertices)
        {
            List<Vector2> midEdgePoints = new List<Vector2>();
            midEdgePoints.Add((_vertices[0] + _vertices[3]) / 2);
            for (int i = 0; i < _vertices.Length - 1; i++)
            {
                midEdgePoints.Add((_vertices[i] + _vertices[i + 1]) / 2);
            }
            return midEdgePoints;
        }
        public static Quad[] SubdividePolygonInQuads(this Vector2[] _vertices)
        {
            Vector2 center = GetPolygonCenter(_vertices);
            List<Vector2> midEdgePoints = GetMidEdgePoints(_vertices);
            Quad[] quads = new Quad[4];
            for (int i = 1; i < midEdgePoints.Count; i++)
            {
                quads[i] = new Quad(center, midEdgePoints[i - 1], midEdgePoints[i], _vertices[i]);
            }

            quads[0] = new Quad(center, midEdgePoints[3], midEdgePoints[0], _vertices[0]);
            return quads;
        }
        
    }
}