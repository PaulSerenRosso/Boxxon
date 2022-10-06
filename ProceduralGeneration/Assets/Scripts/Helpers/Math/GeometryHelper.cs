using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeometryHelpers
{
    public static class GeometryHelper
    {
        public static Circle GetTriangleCircumCircle(this Triangle _triangle)
        {
            float angleBAC = _triangle.GetTriangleVerticeAngle(0);
            float angleABC = _triangle.GetTriangleVerticeAngle(1);
            float angleBCA = Mathf.PI - (angleABC + angleBAC);
            float doubleSinAngleBAC = Mathf.Sin(angleBAC) * 2;
            float doubleSinAngleABC = Mathf.Sin(angleABC) * 2;
            float doubleSinAngleBCA = Mathf.Sin(angleBCA) * 2;
            float circumCircleRadius = angleBAC / doubleSinAngleBAC;
            float circumCircleCenterX = (_triangle.Vertices[0].x * doubleSinAngleBAC +
                                         _triangle.Vertices[1].x * doubleSinAngleABC +
                                         _triangle.Vertices[2].x * doubleSinAngleBCA) /
                                        (doubleSinAngleABC + doubleSinAngleBAC + doubleSinAngleBCA);
            float circumCircleCenterY = (_triangle.Vertices[0].y * doubleSinAngleBAC +
                                         _triangle.Vertices[1].y * doubleSinAngleABC +
                                         _triangle.Vertices[2].y * doubleSinAngleBCA) /
                                        (doubleSinAngleABC + doubleSinAngleBAC + doubleSinAngleBCA);
            Vector2 circumCircleCenter = new Vector2(circumCircleCenterX, circumCircleCenterY);
            return new Circle(circumCircleCenter, circumCircleRadius);
        }

        public static float GetTriangleVerticeAngle(this Triangle _triangle, int vertex)
        {
            float angle = 0;
            switch (vertex)
            {
                case 0:
                {
                    angle = Mathf.Acos(Vector2.Dot((_triangle.Vertices[0] - _triangle.Vertices[1]).normalized,
                        (_triangle.Vertices[0] - _triangle.Vertices[2]).normalized));
                    break;
                }
                case 1:
                {
                    angle = Mathf.Acos(Vector2.Dot((_triangle.Vertices[1] - _triangle.Vertices[0]).normalized,
                        (_triangle.Vertices[1] - _triangle.Vertices[2]).normalized));
                    break;
                }
                case 2:
                {
                    angle = Mathf.Acos(Vector2.Dot((_triangle.Vertices[2] - _triangle.Vertices[0]).normalized,
                        (_triangle.Vertices[2] - _triangle.Vertices[1]).normalized));
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

        public static Triangle GetTriangleWitchInscribesRect(Rect _rect, float _sizeOfTriangleBaseOffset)
        {
            Vector2 vertexA = new Vector2(_rect.xMin - _sizeOfTriangleBaseOffset, _rect.yMin);
            Vector2 vertexB = new Vector2(_rect.xMax + _sizeOfTriangleBaseOffset, _rect.yMin);
            Vector2 vertexD = new Vector2(_rect.xMin, _rect.yMax);
            float cosBAD = _sizeOfTriangleBaseOffset / Vector2.Distance(vertexA, vertexD);
            float distanceAC = (_sizeOfTriangleBaseOffset + _rect.width / 2) / cosBAD;
            Vector2 directionAC = new Vector2(cosBAD, Mathf.Sin(Mathf.Acos(cosBAD)));
            Vector2 vertexC = vertexA + directionAC * distanceAC;
            return new Triangle(vertexA, vertexB, vertexC);
        }

        public static Segment[] GetSegmentsInTriangles(this Triangle _triangle)
        {
            return new Segment[3]
            {
                new(_triangle.Vertices[0], _triangle.Vertices[1]), new(_triangle.Vertices[0], _triangle.Vertices[2]),
                new(_triangle.Vertices[1], _triangle.Vertices[2])
            };
        }

        public static bool TriangleHasEdge(this Triangle _triangle, Segment _segment)
        {
            for (int j = 0; j < _segment.Points.Length; j++)
            {
            for (int i = 0; i < _triangle.Vertices.Length; i++)
            {
                int sharedVerticesCount = 0;
                if (_triangle.Vertices[i] == _segment.Points[j])
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
        public static bool TrianglesHaveOneSharedVertex(this Triangle _triangleA, Triangle _triangleB)
        {
            for (int i = 0; i < _triangleA.Vertices.Length; i++)
            {
                for (int j = 0; j < _triangleB.Vertices.Length; j++)
                {
                    if (_triangleA.Vertices[i] == _triangleB.Vertices[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public static Vector2 GetCommunVertexOfTriangles(this Triangle _triangleA, Triangle _triangleB)
        {
            for (int i = 0; i < _triangleA.Vertices.Length; i++)
            {
                for (int j = 0; j < _triangleB.Vertices.Length; j++)
                {
                    if (_triangleA.Vertices[i] == _triangleB.Vertices[j])
                    {
                        return _triangleA.Vertices[i];
                    }
                }
            }
            throw new Exception("Triangles don't have one same vertex");
        }

        public static Quad CreateQuadWithTwoTriangle(this Triangle _triangleA, Triangle _triangleB, Segment _communEdge)
        {
            Triangle[] triangles = new[] { _triangleA, _triangleB };
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

        public static bool TrianglesHaveOneSameEdge(this Triangle _triangleA, Triangle _triangleB)
        {
            int verticesCount = 0;
            for (int i = 0; i < _triangleA.Vertices.Length; i++)
            {
                for (int j = 0; j < _triangleB.Vertices.Length; j++)
                {
                    if (_triangleA.Vertices[i] == _triangleB.Vertices[j])
                    {
                        verticesCount++;
                    }
                }
            }
            return verticesCount == 2;
        }
        
        public static Segment GetSameEdgeOfTriangles(this Triangle _triangleA, Triangle _triangleB)
        {
            List<Vector2> sameVertices = new List<Vector2>();
            for (int i = 0; i < _triangleA.Vertices.Length; i++)
            {
                for (int j = 0; j < _triangleB.Vertices.Length; j++)
                {
                    if (_triangleA.Vertices[i] == _triangleB.Vertices[j])
                    {
                        sameVertices.Add(_triangleA.Vertices[i]);
                    }
                }
            }
            if (sameVertices.Count == 2)
            {
                return new Segment(sameVertices[0], sameVertices[1]); 
            }
            throw new Exception("Triangles don't have one same edge");
        }
        /*
    public static Quad[] SubdivideQuadInQuads(this Quad _quadA)
    {
        
    }
    
    public static Quad[] SubdivideTriangleInQuads(this Triangle _triangleA)
    {
        
    }
    */
}
    }
    
