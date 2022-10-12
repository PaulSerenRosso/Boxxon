
using UnityEngine;
using UnityEngine.Rendering;

namespace MeshGenerator
{
	public static class MeshGeneratorHelper
	{
		static void GenerateMesh<T>(Mesh mesh, int resolution) where T : struct, IMeshGenerator
		{
			Mesh.MeshDataArray _meshDataArray = Mesh.AllocateWritableMeshData(1);
			Mesh.MeshData meshData = _meshDataArray[0];

			MeshJob<T>.ScheduleParallel(
				mesh, meshData, resolution, default
			).Complete();
			Mesh.ApplyAndDisposeWritableMeshData(_meshDataArray, mesh);
		}
	}
}