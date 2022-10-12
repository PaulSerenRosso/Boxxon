using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace MeshGenerator
{


	public static class SingleStreamHelper
	{

		public static void SetupMeshDataForJobs(
			Mesh.MeshData _meshData, Bounds _bounds, int _vertexCount, int _indexCount
		)
		{
			SetVertexAndTriangleBuffer(_meshData, _vertexCount, _indexCount);

			CreateSubMesh(_meshData, _bounds, _vertexCount, _indexCount);
		}

		private static void CreateSubMesh(Mesh.MeshData _meshData, Bounds _bounds, int _vertexCount, int _indexCount)
		{
			_meshData.subMeshCount = 1;
			_meshData.SetSubMesh(
				0, new SubMeshDescriptor(0, _indexCount)
				{
					bounds = _bounds,
					vertexCount = _vertexCount
				},
				MeshUpdateFlags.DontRecalculateBounds |
				MeshUpdateFlags.DontValidateIndices
			);
		}

		private static void SetVertexAndTriangleBuffer(Mesh.MeshData _meshData, int _vertexCount, int _indexCount)
		{
			var descriptor = new NativeArray<VertexAttributeDescriptor>(
				4, Allocator.Temp, NativeArrayOptions.UninitializedMemory
			);
			descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
			descriptor[1] = new VertexAttributeDescriptor(
				VertexAttribute.Normal, dimension: 3
			);
			descriptor[2] = new VertexAttributeDescriptor(
				VertexAttribute.Tangent, dimension: 4
			);
			descriptor[3] = new VertexAttributeDescriptor(
				VertexAttribute.TexCoord0, dimension: 2
			);
			_meshData.SetVertexBufferParams(_vertexCount, descriptor);
			descriptor.Dispose();

			_meshData.SetIndexBufferParams(_indexCount, IndexFormat.UInt16);
		}

	}
}