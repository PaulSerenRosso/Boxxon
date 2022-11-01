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
        private Vector3[] pointObjectsPosition;

        public void LaunchOrganicGridPointGeneration(OrganicGridCoordinates _organicGridCoordinates)
        {
            PoissonDiskSampling pointGenerator =
                new PoissonDiskSampling(_organicGridCoordinates.GridRect.size, minDistanceBetweenPoint,
                    maxDistanceBetweenPoint);
            points = pointGenerator.GeneratePoints();
            pointObjectsPosition =
                ConvertPointsToPointObjectsPosition(_organicGridCoordinates.StartPosition, _organicGridCoordinates);
            pointObjectGenerator.LaunchPointObjectGenerator(_pointObjectPrefab, pointObjectsPosition);
        }

        private Vector3[] ConvertPointsToPointObjectsPosition(Vector3 _organicGridPosition,
            OrganicGridCoordinates _organicGridCoordinates)
        {
            Vector3[] pointObjectsPosition = new Vector3[points.Length];
                Vector2 gridRectSize = _organicGridCoordinates.GridRect.size;
            for (int i = 0; i < points.Length; i++)
            {
                pointObjectsPosition[i] =
                    points[i].ConvertTo3dSpace(CoordinateType.X, CoordinateType.Z,
                        -new Vector3(gridRectSize.x, 0,
                            gridRectSize.y) / 2);
            }

            return pointObjectsPosition;
        }

        public GameObject[] GetPointObjects()
        {
            return pointObjectGenerator.GetPointObjects();
        }

        public Vector2[] GetPoint2DPosition()
        {
            return points;
        }

        public Vector3[] GetPoint3DPosition()
        {
            return pointObjectsPosition;
        }
    }
}