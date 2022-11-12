using System;
using System.Collections;
using UnityEngine;

namespace PointGenerator
{
public class PointGeneratorObjectFactory : MonoBehaviour
{
   private GameObject[] pointObjects;
   private GameObject pointObjectPrefab;
   [SerializeField]
   private float onePointObjectGenerationTime = 1;

   [SerializeField] private float sizeOfPointObjects;
   public GameObject[] GetPointObjects()
    {
       if (pointObjects.Length == 0)
          throw new Exception("PointsObjects is null, launch Generator before");
       return pointObjects;
    }
   public void CreatePointGeneratorObject(GameObject _pointObjectPrefab, Vector3[] _pointObjectsPosition, Vector3 _offset)
   {
      if (_pointObjectPrefab == null)
         throw new Exception("Prefab is not assigned");
      pointObjectPrefab = _pointObjectPrefab;
    
      StartCoroutine(IterateGeneration(_pointObjectsPosition, _offset));
   }
  
   IEnumerator IterateGeneration(Vector3[] _pointObjectsPosition, Vector3 _offset)
   {
     GameObject[] currentPointObjects = new GameObject[_pointObjectsPosition.Length];
      for (int i = 0; i <  _pointObjectsPosition.Length; i++)
      {
        currentPointObjects[i] = Instantiate(pointObjectPrefab, _pointObjectsPosition[i]+_offset, Quaternion.identity, transform);
        currentPointObjects[i].transform.SetGlobalScale(Vector3.one*sizeOfPointObjects);
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
