using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Map
{
public class PointObjectGenerator : MonoBehaviour
{
  
   [SerializeField]
   private Vector2 gridSize = new Vector3(10,10);
   
   [SerializeField]
   private float minDistanceBetweenPoint = 1;
   [SerializeField]
   private float maxDistanceBetweenPoint = 2 ;
   [SerializeField]
   private GameObject pointObjectPrefab;

   [SerializeField] private GameObject gridObjectPrefab;
   [SerializeField] private Transform parentSpawn;

   [SerializeField]
   private float gridObjectSizeY;

   [SerializeField]
   private float onePointObjectGenerationTime = 1;

   
   
   public void LaunchPointObjectGenerator()
   {
      if (pointObjectPrefab == null || gridObjectPrefab == null)
         throw new Exception("Prefabs are not assigned");
      if (parentSpawn == null)
         throw new Exception("parentSpawn is not assigned");
      GameObject gridObject = Instantiate(gridObjectPrefab, new Vector3(gridSize.x/2,0,gridSize.y/2), quaternion.identity, parentSpawn);
      gridObject.transform.SetGlobalScale(
         new Coordinate[]{new Coordinate(CoordinateType.X, gridSize.x),
         new Coordinate(CoordinateType.Y, gridObjectSizeY),
         new Coordinate(CoordinateType.Z, gridSize.y) 
         });
      GeneratePointObjects();
   }
   

   void GeneratePointObjects()
   {
      StartCoroutine(IterateGeneration(GeneratePointObjectsPosition()));
   }
   
   
   Vector3[] GeneratePointObjectsPosition()
   {
      PointGenerator pointGenerator = new PointGenerator(new Vector2(gridSize.x, gridSize.y), minDistanceBetweenPoint, maxDistanceBetweenPoint);  
      List<Vector2> points = pointGenerator.GeneratePoints();
      Vector3[] pointObjectPositions = new Vector3[points.Count];
      for (int i = 0; i < points.Count; i++)
      {
         pointObjectPositions[i] = new Vector3(points[i].x, 0, points[i].y);
      }
      return pointObjectPositions;
   }

   IEnumerator IterateGeneration(Vector3[] pointObjectPositions)
   {
      for (int i = 0; i <  pointObjectPositions.Length; i++)
      {
         Instantiate(pointObjectPrefab, pointObjectPositions[i], Quaternion.identity, parentSpawn);
         yield return new WaitForSeconds(onePointObjectGenerationTime);
      }
   }
}
   
}
