
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;
using GeometryHelpers;
namespace MeshGenerator
{
	public static class MeshGeneratorHelper
	{
		static void GenerateMesh(IMeshGenerator _generator, Mesh _mesh, int _innerloopBatchCount) 
		{
			Mesh.MeshDataArray _meshDataArray = Mesh.AllocateWritableMeshData(1);
			Mesh.MeshData meshData = _meshDataArray[0];
			MeshJob job = new MeshJob(_generator, _mesh, meshData);
			JobHandle jobHandle = new JobHandle(); 
			job.ScheduleParallel(_generator.JobLength, _innerloopBatchCount, jobHandle);
			jobHandle.Complete();
			Mesh.ApplyAndDisposeWritableMeshData(_meshDataArray, _mesh);
		}
		static Mesh GenerateTriangleMesh(Triangle2DPosition[] _triangles, int _innerloopBatchCount)
		{
			Mesh mesh = new Mesh();
			TriangleMeshGenerator generator = new TriangleMeshGenerator();
			generator.SetUp(_triangles);
			GenerateMesh(generator,mesh,  _innerloopBatchCount);
			return mesh;
		}
		
	}
}