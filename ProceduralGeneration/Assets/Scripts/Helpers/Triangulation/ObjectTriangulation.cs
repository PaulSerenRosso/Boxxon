
using MeshGenerator;
using UnityEngine;

namespace Triangulation
{
public class ObjectTriangulation : MonoBehaviour
{
    [SerializeField] private int innerloopBatchCount;
    [SerializeField] private GameObject triangulationObjectPrefab;
    public void LaunchObjectTriangulation(TriangleID[] _trianglesId, Vector3[] _points, Bounds _bounds,Vector3 _offset )
    {
       Mesh mesh = MeshGeneratorHelper.GenerateTriangleGridMesh(_trianglesId, _points, _bounds, innerloopBatchCount);
     // Mesh mesh = MeshGeneratorHelper.GenerateGridMesh();
      GameObject triangulationObject = Instantiate(triangulationObjectPrefab, _offset+_bounds.center, Quaternion.identity, transform);
      triangulationObject.GetComponent<MeshFilter>().mesh = mesh;
    }
}
}
