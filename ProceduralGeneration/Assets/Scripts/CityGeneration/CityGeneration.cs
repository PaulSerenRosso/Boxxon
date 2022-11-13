using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFDHelper;
using OrganicGrid;
using GeometryHelpers;
using Random = UnityEngine.Random;

namespace CityGeneratorHelper
{
    public class CityGeneration : MonoBehaviour
    {
        [SerializeField] OrganicGridStepLauncher organicGridStepLauncher;
        [SerializeField] List<CityTileType> cityTileTypes;
        [SerializeField] GameObject cityTilePrefab;
        [SerializeField] private GameObject debugPointObject;
        
        void Start()
        {
            QuadID[] finalQuadsID = organicGridStepLauncher.GetFinalQuadID();
            Vector3 offset = organicGridStepLauncher.GetOrganicGridCoordinates().StartPosition;
            Vector3[] finalPoint3D = organicGridStepLauncher.GetFinalPoints3D();
            FFD3D ffd3D = new FFD3D();
            
            for (int i = 0; i < finalQuadsID.Length; i++)
            {
                //To get the center of the quad on the grid
                QuadID currentFinalQuad = finalQuadsID[i];
                Vector3[] currentFinalVerticesInLocalCenterSpace = new Vector3[4];
                Vector3[] currentFinalVertices = new[] {finalPoint3D[currentFinalQuad.VerticesIndex.x], 
                    finalPoint3D[currentFinalQuad.VerticesIndex.y], 
                    finalPoint3D[currentFinalQuad.VerticesIndex.z],
                    finalPoint3D[currentFinalQuad.VerticesIndex.w],
                };
                for (int j = 0; j < currentFinalVertices.Length; j++)
                {
                    currentFinalVertices[j] += offset;
                }
                Vector3 currentFinalQuadCenter = GeometryHelper.GetPolygonCenter(currentFinalVertices);
                
                
                //
                GameObject newCityTile = Instantiate(cityTilePrefab, currentFinalQuadCenter, Quaternion.identity, transform);
                int randomIndexCityTile = Random.Range(0,cityTileTypes.Count);
                MeshFilter newCityTileMeshFilter = newCityTile.GetComponent<MeshFilter>();
                newCityTileMeshFilter.mesh = cityTileTypes[randomIndexCityTile].MeshType;

                float currentFinalQuadArea = GeometryHelper.GetAreaQuad(currentFinalVertices);
                float yPositionForTopControlPoint =
                    currentFinalQuadArea * cityTileTypes[randomIndexCityTile].RatioAreaYSize;
                Vector3 yPositionForTopControlVector = Vector3.up * yPositionForTopControlPoint; 

                //Debug.Log("currentFinalQuadArea :" + currentFinalQuadArea + " " + "yPositionForTopControlPoint : " + yPositionForTopControlPoint + " " +  "yPositionForTopControlVector : " + yPositionForTopControlVector + "cityTileTypes[randomIndexCityTile].RatioAreaYSize" + cityTileTypes[randomIndexCityTile].RatioAreaYSize);
                
                for (int j = 0; j < currentFinalVerticesInLocalCenterSpace.Length; j++)
                {
                    currentFinalVerticesInLocalCenterSpace[j]  = currentFinalVertices[j] - currentFinalQuadCenter;
                    Instantiate(debugPointObject, currentFinalQuadCenter + currentFinalVerticesInLocalCenterSpace[j], Quaternion.identity);
                    //INstantiate Y
                    Instantiate(debugPointObject, currentFinalQuadCenter + currentFinalVerticesInLocalCenterSpace[j] + yPositionForTopControlVector, Quaternion.identity);
                }

                List<Vector3> currentFinalVerticesInLocalCenterSpaceWhichMustAttachToPointBounds =
                    new List<Vector3>(currentFinalVerticesInLocalCenterSpace);
                
                List<Vector3> currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds = 
                    new List<Vector3>();
                for (int j = 0; j < cityTileTypes[randomIndexCityTile].Bounds2DVertices.Length; j++)
                {
                    int vertexCloserCurrentPointBoundsIndex = 0;
                    Vector3 currentPointBounds = cityTileTypes[randomIndexCityTile].Bounds2DVertices[j];
                    float sqrtDistanceBetweenVertexCloseAndPointBounds = (currentPointBounds - currentFinalVerticesInLocalCenterSpaceWhichMustAttachToPointBounds[0]).sqrMagnitude;
                    for (int k = 1; k < currentFinalVerticesInLocalCenterSpaceWhichMustAttachToPointBounds.Count; k++)
                    {
                        float sqrtDistanceBetweenCurrentVertexCloseAndPointBounds = (currentPointBounds - currentFinalVerticesInLocalCenterSpaceWhichMustAttachToPointBounds[k]).sqrMagnitude;
                        if (sqrtDistanceBetweenCurrentVertexCloseAndPointBounds < sqrtDistanceBetweenVertexCloseAndPointBounds)
                        {
                            vertexCloserCurrentPointBoundsIndex = k;
                            sqrtDistanceBetweenVertexCloseAndPointBounds =
                                sqrtDistanceBetweenCurrentVertexCloseAndPointBounds;
                        }
                    }
                    //Add in order to the list
                    currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds.Add(currentFinalVerticesInLocalCenterSpaceWhichMustAttachToPointBounds[vertexCloserCurrentPointBoundsIndex]);
                    //Then we remove 
                    currentFinalVerticesInLocalCenterSpaceWhichMustAttachToPointBounds.RemoveAt(
                        vertexCloserCurrentPointBoundsIndex);
                    Debug.Log("vertexCloserCurrentPointBoundsIndex" + vertexCloserCurrentPointBoundsIndex);
                }


                ffd3D.Launch(newCityTileMeshFilter.mesh, new Vector3[]{
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[0], 
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[1], 
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[0] + yPositionForTopControlVector, 
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[1] + yPositionForTopControlVector, 
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[2],
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[3],
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[2] + yPositionForTopControlVector,
                        currentFinalVerticesInLocalCenterSpaceAttachedToPointBounds[3] + yPositionForTopControlVector
                       },
                   cityTileTypes[randomIndexCityTile].OffsetMesh);
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < cityTileTypes.Count; i++)
            {
                Bounds currentBounds = cityTileTypes[i].MeshType.bounds;
                //Vector3[] bounds2DVertices = new []{
                //    currentBounds.min,
                //    currentBounds.min + Vector3.right * currentBounds.size.x ,
                //    currentBounds.min + Vector3.right * currentBounds.size.x + Vector3.forward * currentBounds.size.z,
                //    currentBounds.min + Vector3.forward * currentBounds.size.z};
                Vector3[] bounds2DVertices = new []{
                    currentBounds.min,
                    currentBounds.min + Vector3.forward * currentBounds.size.z,
                    currentBounds.min + Vector3.right * currentBounds.size.x,
                    currentBounds.min + Vector3.right * currentBounds.size.x + Vector3.forward * currentBounds.size.z,
                };
                cityTileTypes[i].Bounds2DVertices = bounds2DVertices;
                float areaCityTileType = GeometryHelper.GetAreaQuad(bounds2DVertices);
                //cityTileTypes[i].RatioAreaYSize = currentBounds.size.y/areaCityTileType;
                cityTileTypes[i].RatioAreaYSize = areaCityTileType/currentBounds.size.y;
            }
        }
    }
}
