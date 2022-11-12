using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Unity.Mathematics;
using UnityEngine;
using AlgebraHelpers;
using FlexMatrixHelper;
using UnityEngine.Rendering;

namespace GeometryHelpers
{
    public static class GeometryHelper
    {
        public static Vector2 GetTriangleCentroid(this Triangle2DPosition triangle2DPosition)
        {
            float angleABC = triangle2DPosition.GetTriangleVerticeAngle(1);
            float angleBAC = triangle2DPosition.GetTriangleVerticeAngle(0);
            float doubleSinAngleBAC = Mathf.Sin(angleBAC) * 2;
            float angleBCA = Mathf.PI - (angleABC + angleBAC);
            float doubleSinAngleABC = Mathf.Sin(angleABC) * 2;
            float doubleSinAngleBCA = Mathf.Sin(angleBCA) * 2;
            var vertices = triangle2DPosition.Vertices;
            float centroidX = (vertices[0].x * doubleSinAngleBAC +
                               vertices[1].x * doubleSinAngleABC +
                               vertices[2].x * doubleSinAngleBCA) /
                              (doubleSinAngleABC + doubleSinAngleBAC + doubleSinAngleBCA);
            float centroidY = (vertices[0].y * doubleSinAngleBAC +
                               vertices[1].y * doubleSinAngleABC +
                               vertices[2].y * doubleSinAngleBCA) /
                              (doubleSinAngleABC + doubleSinAngleBAC + doubleSinAngleBCA);
            Vector2 centroid = new Vector2(centroidX, centroidY);
            return centroid;
        }


        public static Circle GetTriangleCircumCircle(this Triangle2DPosition triangle2DPosition)
        {
            return new Circle(GetCircumcircleCenter(triangle2DPosition),
                GetCircumcircleRadius(triangle2DPosition));
        }

        public static float GetCircumcircleRadius(this Triangle2DPosition triangle2DPosition)
        {
            float angleBAC = triangle2DPosition.GetTriangleVerticeAngle(0);
            float edgeLengthBC = (triangle2DPosition.Vertices[1] - triangle2DPosition.Vertices[2]).magnitude;
            float doubleSinAngleBAC = Mathf.Sin(angleBAC) * 2;
            float circumCircleRadius = edgeLengthBC / doubleSinAngleBAC;
            return circumCircleRadius;
        }


        public static Vector2 GetLinesIntersection(Segment _firstSegment, Segment _secondSegment)
        {
            List<LinearEquation> linearEquations = new List<LinearEquation>();
            linearEquations.Add(_firstSegment.GetLinearEquationOfLine());
            linearEquations.Add(_secondSegment.GetLinearEquationOfLine());
            int xIsKnew = -1;
            int yIsKnew = -1;
            for (int i = 0; i < linearEquations.Count; i++)
            {
                if (linearEquations[i].hasX)
                {
                    xIsKnew = i;
                }
                else if (linearEquations[i].hasY)
                {
                    yIsKnew = i;
                }
            }

            if (xIsKnew == -1 && yIsKnew == -1)
            {
                for (int i = 0; i < linearEquations.Count; i++)
                {
                    LinearEquation mediator = linearEquations[i];
                    mediator.b *= -1;
                    linearEquations[i] = mediator;
                }

                FlexMatrix directorCoefficientMatrix = new FlexMatrix(new FlexMatrixLine[]
                    {
                        new FlexMatrixLine(new float[] {linearEquations[0].a, -1}),
                        new FlexMatrixLine(new float[] {linearEquations[1].a, -1})
                    }
                );
                directorCoefficientMatrix = directorCoefficientMatrix.Inverse();

                FlexMatrix originOrderer = new FlexMatrix(new FlexMatrixLine[]
                    {
                        new FlexMatrixLine(new float[] {linearEquations[0].b}),
                        new FlexMatrixLine(new float[] {linearEquations[1].b})
                    }
                );
                return directorCoefficientMatrix.Multiply(originOrderer);
            }

            if (xIsKnew != -1 && yIsKnew == -1)
            {
                float xValue = linearEquations[xIsKnew].x;
                linearEquations.RemoveAt(xIsKnew);
                return new Vector2(xValue, linearEquations[0].a * xValue + linearEquations[0].b);
            }

            if (yIsKnew != -1 && xIsKnew == -1)
            {
                float yValue = linearEquations[yIsKnew].y;
                linearEquations.RemoveAt(yIsKnew);
                return new Vector2((-linearEquations[0].b + yValue) / linearEquations[0].a, yValue);
            }

            return new Vector2(linearEquations[xIsKnew].x, linearEquations[yIsKnew].y);
        }

        public static Vector2 GetCircumcircleCenter(this Triangle2DPosition triangle2DPosition)
        {
            Segment[] mediators = new Segment[2];
            for (int i = 0; i < triangle2DPosition.Vertices.Length - 1; i++)
            {
                mediators[i] = triangle2DPosition.GetMediator(i, i + 1);
            }
            return GetLinesIntersection(mediators[0], mediators[1]);
        }

        public static LinearEquation GetLinearEquationOfLine(this Vector2 _firstPoint, Vector2 _secondPoint)
        {
            float a = 0;
            float b = 0;
            float x = 0;
            float y = 0;
            bool hasY = false;
            bool hasX = false;
            if (Math.Abs(_secondPoint.x - _firstPoint.x) > 0)
            {
                a = (_secondPoint.y - _firstPoint.y) / (_secondPoint.x - _firstPoint.x);

                if (a > 0 || a < 0)
                {
                    b = -(a * _firstPoint.x - _firstPoint.y);
                }
                else
                {
                    y = _firstPoint.y;
                    hasY = true;
                }
            }
            else
            {
                x = _firstPoint.x;
                hasX = true;
            }

            return new LinearEquation(a, b, x, y, hasX, hasY);
        }

        public static LinearEquation GetLinearEquationOfLine(this Segment _segment)
        {
            float a = 0;
            float b = 0;
            float x = 0;
            float y = 0;
            bool hasY = false;
            bool hasX = false;
            Vector2 firstPoint = _segment.Points[0];
            Vector2 secondPoint = _segment.Points[1];
            if (Math.Abs(secondPoint.x - firstPoint.x) > 0)
            {
                a = (secondPoint.y - firstPoint.y) / (secondPoint.x - firstPoint.x);

                if (a > 0 || a < 0)
                {
                    b = -(a * firstPoint.x - firstPoint.y);
                }
                else
                {
                    y = firstPoint.y;
                    hasY = true;
                }
            }
            else
            {
                x = firstPoint.x;
                hasX = true;
            }

            return new LinearEquation(a, b, x, y, hasX, hasY);
        }

        public static Segment GetMediator(this Triangle2DPosition _triangle2DPosition,
            int _firstvertexIndex,
            int _secondVertexIndex)
        {
            Vector2 edgeDirection = _triangle2DPosition.Vertices[_secondVertexIndex] -
                                    _triangle2DPosition.Vertices[_firstvertexIndex];
            Vector2 midEdgePoint = _triangle2DPosition.Vertices[_firstvertexIndex] + (edgeDirection / 2);
            Vector2 secondPoint = midEdgePoint + new Vector2(-edgeDirection.y, edgeDirection.x);
            return new Segment(midEdgePoint, secondPoint);
        }


        public static float GetTriangleVerticeAngle(this Triangle2DPosition triangle2DPosition, int vertex)
        {
            float angle = 0;
            switch (vertex)
            {
                case 0:
                {
                    angle = Mathf.Acos(Vector2.Dot(
                        (triangle2DPosition.Vertices[0] - triangle2DPosition.Vertices[1]).normalized,
                        (triangle2DPosition.Vertices[0] - triangle2DPosition.Vertices[2]).normalized));
                    break;
                }
                case 1:
                {
                    angle = Mathf.Acos(Vector2.Dot(
                        (triangle2DPosition.Vertices[1] - triangle2DPosition.Vertices[0]).normalized,
                        (triangle2DPosition.Vertices[1] - triangle2DPosition.Vertices[2]).normalized));
                    break;
                }
                case 2:
                {
                    angle = Mathf.Acos(Vector2.Dot(
                        (triangle2DPosition.Vertices[2] - triangle2DPosition.Vertices[0]).normalized,
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

        public static Segment[] GetSegmentsInTriangles(this Triangle2DPosition _triangle2DPosition)
        {
            return new Segment[3]
            {
                new(_triangle2DPosition.Vertices[0], _triangle2DPosition.Vertices[1]),
                new(_triangle2DPosition.Vertices[1], _triangle2DPosition.Vertices[2]),
                new(_triangle2DPosition.Vertices[2], _triangle2DPosition.Vertices[0])
            };
        }

        public static float GetSubtractOfTriangleAreaAndSubTrianglesAreaComposedWithPoint(
            this Triangle2DPosition _triangle2DPosition, Vector2 _point)
        {
            float area = GetArea(_triangle2DPosition);
            float sumOfSubTriangleArea = 0;

            Vector2[] vertices = _triangle2DPosition.Vertices;
            Triangle2DPosition[] subTriangles = new Triangle2DPosition[]
            {
                new(_point, vertices[0], vertices[1]),
                new(_point, vertices[0], vertices[2]),
                new(_point, vertices[1], vertices[2])
            };
            for (int i = 0; i < subTriangles.Length; i++)
            {
                sumOfSubTriangleArea += subTriangles[i].GetArea();
            }

            return Mathf.Abs(area - sumOfSubTriangleArea);
        }

        public static float GetArea(this Triangle2DPosition triangle2DPosition)
        {
            return Vector3.Cross(triangle2DPosition.Vertices[1] - triangle2DPosition.Vertices[0],
                triangle2DPosition.Vertices[2] - triangle2DPosition.Vertices[0]).magnitude * 0.5f;
        }

        public static bool TriangleHasEdge(this Triangle2DPosition triangle2DPosition, Segment _segment)
        {
            int sharedVerticesCount = 0;
            for (int j = 0; j < _segment.Points.Length; j++)
            {
                for (int i = 0; i < triangle2DPosition.Vertices.Length; i++)
                {
                    if (triangle2DPosition.Vertices[i] == _segment.Points[j])
                    {
                        sharedVerticesCount++;
                    }
                }
            }

            if (sharedVerticesCount == 2)
            {
                return true;
            }

            return false;
        }

        public static int GetSharedVertices(this Segment _firstSegment, Segment _secondSegment)
        {
            int sharedVerticesCount = 0;
            for (int j = 0; j < _firstSegment.Points.Length; j++)
            {
                for (int i = 0; i < _secondSegment.Points.Length; i++)
                {
                    if (_firstSegment.Points[j] == _secondSegment.Points[i])
                    {
                        sharedVerticesCount++;
                    }
                }
            }

            return sharedVerticesCount;
        }

        public static readonly Triangle2DPosition Triangle2DPositionZero =
            new Triangle2DPosition(Vector2.zero, Vector2.zero, Vector2.zero);

        public static bool TrianglesHaveOneSharedVertex(this Triangle2DPosition triangle2DPositionA,
            Triangle2DPosition triangle2DPositionB)
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

        public static bool TrianglesHaveTwoSharedVertices(this Triangle2DPosition triangle2DPositionA,
            Triangle2DPosition triangle2DPositionB)
        {
            int sharedVerticesCount = 0;
            for (int i = 0; i < triangle2DPositionA.Vertices.Length; i++)
            {
                for (int j = 0; j < triangle2DPositionB.Vertices.Length; j++)
                {
                    if (triangle2DPositionA.Vertices[i] == triangle2DPositionB.Vertices[j])
                    {
                        sharedVerticesCount++;
                        break;
                    }
                }
            }

            if (sharedVerticesCount == 2)
            {
                return true;
            }


            return false;
        }

        public static List<Vector2> GetSharedVertices(this Triangle2DPosition triangle2DPositionA,
            Triangle2DPosition triangle2DPositionB)
        {
            List<Vector2> sharedVertices = new List<Vector2>();
            for (int i = 0; i < triangle2DPositionA.Vertices.Length; i++)
            {
                for (int j = 0; j < triangle2DPositionB.Vertices.Length; j++)
                {
                    if (triangle2DPositionA.Vertices[i] == triangle2DPositionB.Vertices[j])
                    {
                        sharedVertices.Add(triangle2DPositionA.Vertices[i]);
                        break;
                    }
                }
            }

            return sharedVertices;
        }


        public static float ConvertDirectionToSignedAngle(Vector2 _firstPoint, Vector2 _secondPoint)
        {
            Vector2 directionOppositeAngleOfCurrentTriangleToCurrentPoint =
                (_secondPoint - _firstPoint).normalized;
            return Mathf.Atan2(
                directionOppositeAngleOfCurrentTriangleToCurrentPoint.y,
                directionOppositeAngleOfCurrentTriangleToCurrentPoint.x);
        }

        public static float ConvertToSignedAngleToPositiveAngle(float _angle)
        {
            if (_angle < 0)
            {
                _angle =
                    Mathf.PI * 2 + _angle;
            }

            return _angle;
        }


        public static Vector2 GetCommunVertexOfTriangles(this Triangle2DPosition triangle2DPositionA,
            Triangle2DPosition triangle2DPositionB)
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

        public static Quad2DPosition CreateQuadWithTwoTriangle(this Triangle2DPosition triangle2DPositionA,
            Triangle2DPosition triangle2DPositionB, Segment _communEdge)
        {
            Triangle2DPosition[] triangles = new[] {triangle2DPositionA, triangle2DPositionB};
            //check pour 
            List<Vector2> unsharedPoints = new List<Vector2>();

            for (int j = 0; j < triangles.Length; j++)
            {
                for (int k = 0; k < triangles[j].Vertices.Length; k++)
                {
                    if (triangles[j].Vertices[k] != _communEdge.Points[0] &&
                        triangles[j].Vertices[k] != _communEdge.Points[1])
                    {
                        unsharedPoints.Add(triangles[j].Vertices[k]);
                    }
                }
            }

            if (unsharedPoints.Count != 2)
                throw new Exception("Triangles have not one same edge or the current segment is not the good one");
            return new Quad2DPosition(_communEdge.Points[0], unsharedPoints[0], _communEdge.Points[1],
                unsharedPoints[1]);
        }

        public static bool IsCounterClockwise(Vector2 _a, Vector2 _b, Vector2 _c)
        {
            var value = (_b.x - _a.x) * (_c.y - _a.y) - (_c.x - _a.x) * (_b.y - _a.y);
            if (value > 0)
            {
                return true;
            }

            if (value == 0)
            {
                throw new Exception("a b and c are colinears");
            }

            return false;
        }

        public static bool IsCounterClockwise(this Triangle2DPosition triangle)
        {
            Vector2[] vertices = triangle.Vertices;
            Vector2 a = vertices[0];
            Vector2 b = vertices[1];
            Vector2 c = vertices[2];
            var value = (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
            if (value > 0)
            {
                return true;
            }

            if (value == 0)
            {
                throw new Exception("a b and c are colinears");
            }

            return false;
        }

        public static bool CheckIfPolygonIsConvex(Vector2[] _vertices)
        {
            if (_vertices.Length < 4)
            {
                throw new Exception("polygon must have more than 3 vertices");
            }

            bool isZNegative = false;

            Vector3 firstCrossProduct =
                Vector3.Cross(_vertices[_vertices.Length - 1] - _vertices[0], _vertices[0] - _vertices[1]);


            if (firstCrossProduct.z < 0)
            {
                isZNegative = true;
            }


            Vector3 lastCrossProduct = Vector3.Cross(_vertices[_vertices.Length - 1] - _vertices[0],
                _vertices[_vertices.Length - 1] - _vertices[_vertices.Length - 2]);

            if ((lastCrossProduct.z > 0 && isZNegative) || (lastCrossProduct.z < 0 && !isZNegative))
            {
                return false;
            }

            for (int i = 1; i < _vertices.Length - 1; i++)
            {
                Vector3 currentCrossProduct = Vector3.Cross(_vertices[i] - _vertices[i - 1],
                    _vertices[i + 1] - _vertices[i]);
                if ((currentCrossProduct.z > 0 && isZNegative) || (currentCrossProduct.z < 0 && !isZNegative))
                {
                    return false;
                }
            }


            return true;
        }

        public static bool CheckIfPolygonConvexHasAllItsAnglesClamped(Vector2[] _vertices, float _minAngle,
            float _maxAngle)
        {
            List<Vector2[]> directionsForAngle = new List<Vector2[]>();

            directionsForAngle.Add(
                new[] {_vertices[_vertices.Length - 1] - _vertices[0], _vertices[1] - _vertices[0]});
            directionsForAngle.Add(new[]
            {
                _vertices[0] - _vertices[_vertices.Length - 1],
                _vertices[_vertices.Length - 2] - _vertices[_vertices.Length - 1]
            });
            for (int i = 1; i < _vertices.Length - 1; i++)
            {
                directionsForAngle.Add(new[] {_vertices[i - 1] - _vertices[i], _vertices[i + 1] - _vertices[i]});
            }

            for (int i = 0; i < directionsForAngle.Count; i++)
            {
                if (!Vector2.Angle(directionsForAngle[i][0], directionsForAngle[i][1]).IsClamp(_minAngle, _maxAngle))
                {
                    return false;
                }
            }

            return true;
        }


        public static bool TrianglesHaveOneSameEdge(this Triangle2DPosition triangle2DPositionA,
            Triangle2DPosition triangle2DPositionB)
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

        public static Segment GetSameEdgeOfTriangles(this Triangle2DPosition triangle2DPositionA,
            Triangle2DPosition triangle2DPositionB)
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
            midEdgePoints.Add(_vertices[_vertices.Length - 1] + ((_vertices[0] - _vertices[_vertices.Length - 1]) / 2));
            for (int i = 0; i < _vertices.Length - 1; i++)
            {
                midEdgePoints.Add(_vertices[i + 1] + ((_vertices[i] - _vertices[i + 1]) / 2));
            }

            return midEdgePoints;
        }

        public static Vector2 GetTheOppositeVertexToTheEdge(this Triangle2DPosition _triangle, Segment _edge)
        {
            Vector2 oppositeVertex = Vector2.zero;
            for (int j = 0; j < _triangle.Vertices.Length; j++)
            {
                if (_triangle.Vertices[j] != _edge.Points[0]
                    && _triangle.Vertices[j] != _edge.Points[1])
                {
                    oppositeVertex = _triangle.Vertices[j];
                    break;
                }
            }

            return oppositeVertex;
        }
        
    }
    
}