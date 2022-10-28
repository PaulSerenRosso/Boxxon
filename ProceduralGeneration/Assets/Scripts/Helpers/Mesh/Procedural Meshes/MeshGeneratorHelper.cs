
using System;
using Unity.Jobs;
using UnityEngine;
using GeometryHelpers;
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
			
			MeshJobTrianglesAndVertices	trianglesAndVertices = new MeshJobTrianglesAndVertices();
			trianglesAndVertices.Setup(_meshData);
		}
		public static Mesh GenerateTriangleMesh(TriangleID[] _triangles, Vector3[] _points, Bounds _bounds, int _innerloopBatchCount)
		{
			Mesh mesh = new Mesh();
			Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
			Mesh.MeshData meshData = meshDataArray[0];
			TriangleGridMeshGenerator generator = new TriangleGridMeshGenerator();
			SetUpMeshDataBuffer(generator, mesh, meshData);
			generator.SetUp(_points, _triangles, _bounds);
			LaunchMeshJob(_innerloopBatchCount, generator, mesh, meshData);
			Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
			return mesh;
		}
		private static JobHandle LaunchMeshJob(int _innerloopBatchCount, TriangleGridMeshGenerator generator, Mesh mesh,
			Mesh.MeshData meshData)
		{
			
		//	MeshJob<IMeshGenerator> job = new MeshJob(generator, mesh, meshData);
			JobHandle jobHandle = new JobHandle();
		//	job.ScheduleParallel(generator.JobLength, _innerloopBatchCount, jobHandle);
			jobHandle.Complete();
			return jobHandle;
		}
	}
}