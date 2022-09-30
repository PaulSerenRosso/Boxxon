using Unity.Mathematics;
using UnityEngine;

namespace OrganicGrid
{
public class OrganicGridStepLauncher : MonoBehaviour
{
    [SerializeField] private OrganicGridPointGeneration organicGridPointGeneration;
    [SerializeField] private OrganicGridTriangulation organicGridTriangulation;
    [SerializeField] private GameObject baseGridObjectPrefab;
    [SerializeField] private Rect organicGridRect = new Rect(0 ,0, 5,5);
    
    private void Start()
    {
        CreateBaseGridObjectPrefab();
        organicGridPointGeneration.LaunchOrganicGridPointGeneration(organicGridRect);
        organicGridTriangulation.LaunchOrganicGridTriangulation(organicGridRect,organicGridPointGeneration.GetPointObjectsPosition());
    }

    private void CreateBaseGridObjectPrefab()
    {
        GameObject gridObject = Instantiate(baseGridObjectPrefab, new Vector3(organicGridRect.size.x/2,0,organicGridRect.size.y/2), quaternion.identity, transform);
        gridObject.transform.SetGlobalScale(
            new Coordinate[]{new Coordinate(CoordinateType.X, organicGridRect.size.x),
                new Coordinate(CoordinateType.Y, 0),
                new Coordinate(CoordinateType.Z, organicGridRect.size.y) 
            });
    }
}

}
