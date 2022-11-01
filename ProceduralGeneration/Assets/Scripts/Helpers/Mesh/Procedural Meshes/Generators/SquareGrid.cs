using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

namespace MeshGenerator
{
	
	public struct SquareGrid : IMeshGenerator

	{

	public Bounds Bounds => new Bounds(Vector3.zero, new Vector3(1f, 0f, 1f));

	public int VertexCount => 4 * Resolution * Resolution;

	public int IndexCount => 6 * Resolution * Resolution;

	public int JobLength => Resolution;

	public int Resolution { get; set; }

	public void Execute(int z, MeshJobTrianglesAndVertices _trianglesAndVertices)
	{
		int vi = 4 * Resolution * z, ti = 2 * Resolution * z;

		for (int x = 0; x < Resolution; x++, vi += 4, ti += 2)
		{
			var xCoordinates = float2(x, x + 1f) / Resolution - 0.5f;
			var zCoordinates = float2(z, z + 1f) / Resolution - 0.5f;

			var vertex = new Vertex();
			vertex.Normal.y = 1f;
			vertex.Tangent.xw = float2(1f, -1f);

			vertex.Position.x = xCoordinates.x;
			vertex.Position.z = zCoordinates.x;
			_trianglesAndVertices.SetVertex(vi + 0, vertex);

			vertex.Position.x = xCoordinates.y;
			vertex.TexCoord0 = float2(1f, 0f);
			_trianglesAndVertices.SetVertex(vi + 1, vertex);

			vertex.Position.x = xCoordinates.x;
			vertex.Position.z = zCoordinates.y;
			vertex.TexCoord0 = float2(0f, 1f);
			_trianglesAndVertices.SetVertex(vi + 2, vertex);

			vertex.Position.x = xCoordinates.y;
			vertex.TexCoord0 = 1f;
			_trianglesAndVertices.SetVertex(vi + 3, vertex);

			_trianglesAndVertices.SetTriangle(ti + 0, new int3( vi+0, vi+2, vi+1));
			_trianglesAndVertices.SetTriangle(ti + 1, new int3(vi+1, vi+2, vi+3));
		}
	}
	}
	
}