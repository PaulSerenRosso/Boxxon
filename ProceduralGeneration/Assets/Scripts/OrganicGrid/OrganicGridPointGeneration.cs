using PointGenerator;
using UnityEngine;

namespace OrganicGrid
{
    public class OrganicGridPointGeneration : MonoBehaviour
    {
        [SerializeField] private float minDistanceBetweenPoint = 1;
        [SerializeField] private float maxDistanceBetweenPoint = 2;

        [SerializeField] private PointObjectGenerator pointObjectGenerator;

        [SerializeField] private GameObject _pointObjectPrefab;
        private Vector2[] points; 
        public void LaunchOrganicGridPointGeneration(Rect organicGridRect)
        { 
            PoissonDiskSampling pointGenerator =
                new PoissonDiskSampling(organicGridRect.size, minDistanceBetweenPoint, maxDistanceBetweenPoint);
        points =  pointGenerator.GeneratePoints();
        Vector3[] pointObjectsPosition = ConvertPointsToPointObjectsPosition(organicGridRect);
        pointObjectGenerator.LaunchPointObjectGenerator( _pointObjectPrefab, pointObjectsPosition);
        }

        private Vector3[] ConvertPointsToPointObjectsPosition(Rect organicGridRect)
        {
            Vector3[] pointObjectsPosition = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pointObjectsPosition[i] =
                    points[i].ConvertTo3dSpace(CoordinateType.X, CoordinateType.Z, organicGridRect.min);
            }

            return pointObjectsPosition;
        }

        public GameObject[] GetPointObjects()
        {
            return pointObjectGenerator.GetPointObjects();
        }
        public Vector2[] GetPointObjectsPosition()
        {
            return points;
        }
    }
}
