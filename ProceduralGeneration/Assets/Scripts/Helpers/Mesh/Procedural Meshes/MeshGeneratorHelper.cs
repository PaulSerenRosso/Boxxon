using System;
using Unity.Jobs;
using UnityEngine;
using GeometryHelpers;
using Unity.Collections;

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

        public static Mesh GenerateTriangleMesh(TriangleID[] _triangles, Vector3[] _points, Bounds _bounds,
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
    }
}