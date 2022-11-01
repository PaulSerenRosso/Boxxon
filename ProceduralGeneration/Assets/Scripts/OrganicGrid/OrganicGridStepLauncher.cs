using GeometryHelpers;
using Unity.Mathematics;
using UnityEngine;

namespace OrganicGrid
{
public class OrganicGridStepLauncher : MonoBehaviour
{
    [SerializeField] private OrganicGridPointGeneration organicGridPointGeneration;
    [SerializeField] private OrganicGridTriangulation organicGridTriangulation;
    [SerializeField] private GameObject baseGridObjectPrefab;
    [SerializeField] private Transform startGridTransform;
                  
    [SerializeField] private Vector2 gridSize = new Vector2(5,5);
    [SerializeField] private OrganicGridCoordinates organicGridCoordinates;
    
    private void Start()
    {
        organicGridCoordinates = new OrganicGridCoordinates(new Rect(Vector2.zero, gridSize),
            startGridTransform == null ? Vector3.zero : startGridTransform.position);
        CreateBaseGridObjectPrefab();
        organicGridPointGeneration.LaunchOrganicGridPointGeneration(organicGridCoordinates);
        organicGridTriangulation.LaunchOrganicGridTriangulation(organicGridCoordinates,organicGridPointGeneration.GetPoint2DPosition(),
            organicGridPointGeneration.GetPoint3DPosition());
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
