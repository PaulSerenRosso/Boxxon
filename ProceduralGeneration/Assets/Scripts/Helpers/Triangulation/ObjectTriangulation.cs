
using MeshGenerator;
using UnityEngine;

namespace Triangulation
{
public class ObjectTriangulation : MonoBehaviour
{
    [SerializeField] private int innerloopBatchCount;
    [SerializeField] private GameObject triangulationObjectPrefab;
    public void LaunchObjectTriangulation(TriangleID[] _trianglesId, Vector3[] _points, Bounds _bounds )
    {
       Mesh mesh = MeshGeneratorHelper.GenerateTriangleMesh(_trianglesId, _points, _bounds, innerloopBatchCount);
      GameObject triangulationObject = Instantiate(triangulationObjectPrefab, _bounds.center, Quaternion.identity, transform);
      triangulationObject.GetComponent<MeshFilter>().mesh = mesh;
    }
}
}
