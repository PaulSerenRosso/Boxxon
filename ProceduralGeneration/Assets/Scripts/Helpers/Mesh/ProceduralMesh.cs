using ProceduralMeshes;
using ProceduralMeshes.Streams;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour {

	[SerializeField, Range(1, 10)]
	int resolution = 1;

	Mesh mesh;

	void Awake () {
		mesh = new Mesh {
			name = "Procedural Mesh"
		};
		GetComponent<MeshFilter>().mesh = mesh;
	}

	void OnValidate () => enabled = true;

	void Update () {
		GenerateMesh<SquareGrid>();
		enabled = false;
	}

	void GenerateMesh<T> () where T : struct, IMeshGenerator {
		Mesh.MeshDataArray _meshDataArray = Mesh.AllocateWritableMeshData(1);
		Mesh.MeshData meshData = _meshDataArray[0];

		MeshJob<T>.ScheduleParallel(
			mesh, meshData, resolution, default
		).Complete();
		Mesh.ApplyAndDisposeWritableMeshData(_meshDataArray, mesh);
	}
}