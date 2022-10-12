using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

 namespace MeshGenerator {

	[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
	public struct MeshJob<G> : IJobFor
		where G : struct, IMeshGenerator
		{
			G generator;
		
		[WriteOnly]
		MeshJobTrianglesAndVertices trianglesAndVertices;

		// mettre le constructeur du generator dans le helper
		
		public void Execute (int i) => generator.Execute(i, trianglesAndVertices);

		public static JobHandle ScheduleParallel (
			Mesh _mesh, Mesh.MeshData _meshData, int _resolution, JobHandle _dependency
		) {
			var job = new MeshJob<G>();
			job.generator.Resolution = _resolution;
			SingleStreamHelper.SetupMeshDataForJobs(
				_meshData,
				_mesh.bounds = job.generator.Bounds,
				job.generator.VertexCount,
				job.generator.IndexCount
			);
			job.trianglesAndVertices.Setup(_meshData);
			return job.ScheduleParallel(
				job.generator.JobLength, 1, _dependency
			);
		}
	}
}