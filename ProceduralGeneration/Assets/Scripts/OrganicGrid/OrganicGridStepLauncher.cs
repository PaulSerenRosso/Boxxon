using System.Linq;
using GeometryHelpers;
using Unity.Mathematics;
using UnityEngine;

namespace OrganicGrid
{
public class OrganicGridStepLauncher : MonoBehaviour
{
    [SerializeField] private OrganicGridPointGeneration organicGridPointGeneration;
    [SerializeField] private OrganicGridTriangulation organicGridTriangulation;
    [SerializeField] private OrganicGridTriangleMerger organicGridTriangleMerger;
    [SerializeField] private GameObject baseGridObjectPrefab;
    [SerializeField] private Transform startGridTransform;
                  
    [SerializeField] private Vector2 gridSize = new Vector2(5,5);
    [SerializeField] private OrganicGridCoordinates organicGridCoordinates;
    private Bounds gridBounds;
    private void Start()
    {
        gridBounds = new Bounds(
            organicGridCoordinates.GridRect.center.ConvertTo3dSpace(CoordinateType.X, CoordinateType.Z,
                Vector2.zero),
            new Vector3(organicGridCoordinates.GridRect.size.x, 2, organicGridCoordinates.GridRect.size.y));
        organicGridCoordinates = new OrganicGridCoordinates(new Rect(Vector2.zero, gridSize),
            startGridTransform == null ? Vector3.zero : startGridTransform.position);
        CreateBaseGridObjectPrefab();
        organicGridPointGeneration.LaunchOrganicGridPointGeneration(organicGridCoordinates);
        Vector3[] points3D = organicGridPointGeneration.GetPoint3DPosition();
        organicGridTriangulation.LaunchOrganicGridTriangulation(organicGridCoordinates,organicGridPointGeneration.GetPoint2DPosition(),
            points3D, gridBounds);
        organicGridTriangleMerger.LaunchOrganicGridTriangleMerger(
            organicGridTriangulation.GetFinalTriangles2DPosition().ToList(), organicGridTriangulation.GetFinalTrianglesID(), points3D, gridBounds);
    }

    private void CreateBaseGridObjectPrefab()
    {
        GameObject gridObject = Instantiate(baseGridObjectPrefab, organicGridCoordinates.StartPosition+new Vector3(organicGridCoordinates.GridRect.size.x/2,0,organicGridCoordinates.GridRect.size.y/2), quaternion.identity, transform);
        gridObject.transform.SetGlobalScale(
            new Coordinate[]{new Coordinate(CoordinateType.X, organicGridCoordinates.GridRect.size.x),
                new Coordinate(CoordinateType.Y, 0),
                new Coordinate(CoordinateType.Z, organicGridCoordinates.GridRect.size.y) 
            });
    }
}

}
