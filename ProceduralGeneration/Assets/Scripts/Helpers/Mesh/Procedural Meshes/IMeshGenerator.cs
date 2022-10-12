using UnityEngine;

namespace MeshGenerator {

	public interface IMeshGenerator {

		Bounds Bounds { get; }

		int VertexCount { get; }

		int IndexCount { get; }

		int JobLength { get; }

		int Resolution { get; set; }

		void Execute (int i, MeshJobTrianglesAndVertices _trianglesAndVertices);
		

	}
}