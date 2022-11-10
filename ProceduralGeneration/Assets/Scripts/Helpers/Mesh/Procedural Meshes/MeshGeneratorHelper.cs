using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using UnityEngine;
using GeometryHelpers;
using Unity.Collections;
using Unity.Mathematics;

namespace MeshGenerator
{
    public static class MeshGeneratorHelper
    {
        static void SetUpMeshDataBuffer(IMeshGenerator _generator, Mesh _mesh, Mesh.MeshData _meshData)
        {
            SingleStreamHelper.SetupMeshDataForJobs(
                _meshData,
                _mesh.bounds = _generator.Bounds,
                _generator.VertexCount,
                _generator.IndexCount
            );
        }


        public static Mesh GenerateTriangleGridMesh(TriangleID[] _triangles, Vector3[] _points, Bounds _bounds,
            int _innerloopBatchCount)
        {
            Mesh mesh = new Mesh();
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];
            TriangleGridMeshGenerator generator = new TriangleGridMeshGenerator();
            generator.SetUp(_points, _triangles, _bounds);
            MeshJob<TriangleGridMeshGenerator> job = new MeshJob<TriangleGridMeshGenerator>(generator, mesh, meshData);
            JobHandle jobHandle = new JobHandle();
            jobHandle = job.ScheduleParallel(generator.JobLength, _innerloopBatchCount, default);
            jobHandle.Complete();
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
            return mesh;
        }

        public static Mesh GenerateQuadGridMesh(QuadID[] _quadsId, Vector3[] _points, Bounds _bounds,
            int _innerloopBatchCount)
        {
            Mesh mesh = new Mesh();
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];
            QuadGridMeshGenerator generator = new QuadGridMeshGenerator();
            generator.SetUp(_points, _quadsId, _bounds);
            MeshJob<QuadGridMeshGenerator> job = new MeshJob<QuadGridMeshGenerator>(generator, mesh, meshData);
            JobHandle jobHandle = new JobHandle();
            jobHandle = job.ScheduleParallel(generator.JobLength, _innerloopBatchCount, default);
            jobHandle.Complete();
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
            return mesh;
        }

        public static int SelectIDInTriangleId(this TriangleID _triangleID, int _index)
        {
            int id = -1;
            switch (_index)
            {
                case 0:
                {
                    id = _triangleID.A;
                    break;
                }
                case 1:
                {
                    id = _triangleID.B;
                    break;
                }
                case 2:
                {
                    id = _triangleID.C;
                    break;
                }
            }

            if (id == -1)
            {
                throw new Exception("index is not valid, must be clamp between 0 and 2");
            }

            return id;
        }

        /*
        public static int SelectIDInQuadId(this QuadID _quadID, int _index)
        {
            int id = -1;
            switch (_index)
            {
                case 0:
                {
                    id = _quadID.A;
                    break;
                }
                case 1:
                {
                    id = _quadID.B;
                    break;
                }
                case 2:
                {
                    id = _quadID.C;
                    break;
                }
                case 3:
                {
                    id = _quadID.D;
                    break;
                }
            }

            if (id == -1)
            {
                throw new Exception("index is not valid, must be clamp between 0 and 3");
            }

            return id;
        }
        */

        public static Mesh GenerateGridMesh()
        {
            Mesh mesh = new Mesh();
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];
            SquareGrid generator = new SquareGrid();
            generator.Resolution = 2;
            MeshJob<SquareGrid> job = new MeshJob<SquareGrid>(generator, mesh, meshData);
            JobHandle jobHandle = new JobHandle();
            jobHandle = job.ScheduleParallel(generator.JobLength, 30, default);
            jobHandle.Complete();
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
            return mesh;
        }

        public static TriangleID[] GetTrianglesID(Triangle2DPosition[] _triangles, Vector2[] _points)
        {
            TriangleID[] trianglesId = new TriangleID[_triangles.Length];
            for (int i = 0; i < _triangles.Length; i++)
            {
                int[] ids = new int[3];
                for (int j = 0; j < _triangles[i].Vertices.Length; j++)
                {
                    ids[j] = Array.IndexOf(_points, _triangles[i].Vertices[j]);
                }

                trianglesId[i] = new TriangleID(ids);
            }

            return trianglesId;
        }

        public static QuadID[] GetQuadsID(Quad[] _quads, Vector2[] _points)
        {
            QuadID[] finalQuadIds = new QuadID[_quads.Length];

            for (int i = 0; i < _quads.Length; i++)
            {
                Quad currentQuad = _quads[i];
                Segment[] diagonalsOfQuad = currentQuad.GetDiagonalsOfQuad();
                Segment firstDiagonal = diagonalsOfQuad[0];
                Vector2[] currentQuadVertices = currentQuad.Vertices;
                int firstDiagonalFirstPointIndexInQuadVertices =
                    Array.IndexOf(currentQuadVertices, firstDiagonal.Points[0]);
                int firstDiagonalSecondPointIndexInQuadVertices =
                    Array.IndexOf(currentQuadVertices, firstDiagonal.Points[1]);
                List<TriangleID> trianglesIdForQuadId = new List<TriangleID>();
                List<int> verticesIndexForQuadID = new List<int>();
                for (int j = 0; j < currentQuadVertices.Length; j++)
                {
                    if (j != firstDiagonalFirstPointIndexInQuadVertices &&
                        j != firstDiagonalSecondPointIndexInQuadVertices)
                    {
                        int[] verticesIndexInPointsForTriangleId = new int[3];
                        Triangle2DPosition triangleForQuadId = new Triangle2DPosition(
                            currentQuadVertices[firstDiagonalFirstPointIndexInQuadVertices], currentQuadVertices[j],
                            currentQuadVertices[firstDiagonalSecondPointIndexInQuadVertices]);
                        if (!triangleForQuadId.IsCounterClockwise())
                        {
                            triangleForQuadId = new Triangle2DPosition(
                                currentQuadVertices[j], currentQuadVertices[firstDiagonalFirstPointIndexInQuadVertices],
                                currentQuadVertices[firstDiagonalSecondPointIndexInQuadVertices]);
                        }

                        for (int k = 0; k < 3; k++)
                        {
                            int indexOfVertex = Array.IndexOf(_points, triangleForQuadId.Vertices[k]);
                            verticesIndexInPointsForTriangleId[k] = indexOfVertex;
                            if (!verticesIndexForQuadID.Contains(verticesIndexInPointsForTriangleId[k]))
                            {
                                verticesIndexForQuadID.Add(indexOfVertex);
                            }
                        }

                        trianglesIdForQuadId.Add(new TriangleID(verticesIndexInPointsForTriangleId));
                    }
                }

                finalQuadIds[i] = new QuadID(trianglesIdForQuadId[0], trianglesIdForQuadId[1],
                    new int4(verticesIndexForQuadID[0],verticesIndexForQuadID[1],verticesIndexForQuadID[2],verticesIndexForQuadID[3]));
            }

            return finalQuadIds;
        }
    }
}