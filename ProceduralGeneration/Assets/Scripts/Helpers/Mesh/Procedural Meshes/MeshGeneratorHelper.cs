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


        public static QuadID[] GetQuadsID(Quad2DPosition[] _quads, Vector2[] _points)
        {
            QuadID[] finalQuadIds = new QuadID[_quads.Length];

            for (int i = 0; i < _quads.Length; i++)
            {
                Quad2DPosition currentQuad2DPosition = _quads[i];

                if (!GeometryHelper.IsCounterClockwise(currentQuad2DPosition.Vertices[0],
                    currentQuad2DPosition.Vertices[1], currentQuad2DPosition.Vertices[3]))
                {
                    (currentQuad2DPosition.Vertices[0], currentQuad2DPosition.Vertices[2]) =
                        (currentQuad2DPosition.Vertices[2], currentQuad2DPosition.Vertices[0]);
                }
                finalQuadIds[i] = new QuadID(new int4(Array.IndexOf(_points, currentQuad2DPosition.Vertices[0]),
                    Array.IndexOf(_points, currentQuad2DPosition.Vertices[1]),
                    Array.IndexOf(_points, currentQuad2DPosition.Vertices[2]),
                    Array.IndexOf(_points, currentQuad2DPosition.Vertices[3])));
            }

            return finalQuadIds;
        }
    }
}