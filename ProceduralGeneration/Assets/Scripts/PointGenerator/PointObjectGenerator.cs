using System;
using System.Collections;
using UnityEngine;

namespace PointGenerator
{
public class PointObjectGenerator : MonoBehaviour
{
   private GameObject[] pointObjects;
   private GameObject pointObjectPrefab;
   private float onePointObjectGenerationTime = 1;
   public GameObject[] GetPointObjects()
    {
       if (pointObjects.Length == 0)
          throw new Exception("PointsObjects is null, launch Generator before");
       return pointObjects;
    }
   public void LaunchPointObjectGenerator(GameObject _pointObjectPrefab, Vector3[] _pointObjectsPosition, float _onePointObjectGenerationTime = 1)
   {
      if (_pointObjectPrefab == null)
         throw new Exception("Prefab is not assigned");
      pointObjectPrefab = _pointObjectPrefab;
      onePointObjectGenerationTime = _onePointObjectGenerationTime;
      StartCoroutine(IterateGeneration(_pointObjectsPosition));
   }
  
   IEnumerator IterateGeneration(Vector3[] _pointObjectsPosition)
   {
     GameObject[] currentPointObjects = new GameObject[_pointObjectsPosition.Length];
      for (int i = 0; i <  _pointObjectsPosition.Length; i++)
      {
        currentPointObjects[i] = Instantiate(pointObjectPrefab, _pointObjectsPosition[i], Quaternion.identity, transform);
         yield return new WaitForSeconds(onePointObjectGenerationTime);
      }
      pointObjects = new GameObject[currentPointObjects.Length];
      for (int i = 0; i < currentPointObjects.Length; i++)
      {
         pointObjects[i] = currentPointObjects[i];
      }
   }
}
   
}
