using System.Collections.Generic;
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
        [SerializeField] private OrganicGridSubdivider organicGridSubdivider;
        [SerializeField] private GameObject baseGridObjectPrefab;
        [SerializeField] private Transform startGridTransform;

        [SerializeField] private Vector2 gridSize = new Vector2(5, 5);
        [SerializeField] private OrganicGridCoordinates organicGridCoordinates;
        [SerializeField] private Quad2DPosition[] finalQuads;
        [SerializeField] private QuadID[] finalQuadsID;
        [SerializeField] private Vector3[] finalPoints3D;
        private Bounds gridBounds;

        private void Awake()
        {
            organicGridCoordinates = new OrganicGridCoordinates(new Rect(Vector2.zero, gridSize),
                startGridTransform == null ? Vector3.zero : startGridTransform.position);
            gridBounds = new Bounds(
                Vector3.zero,
                new Vector3(organicGridCoordinates.GridRect.size.x, 2, organicGridCoordinates.GridRect.size.y));
            CreateBaseGridObjectPrefab();
            organicGridPointGeneration.LaunchOrganicGridPointGeneration(organicGridCoordinates);
            Vector3[] points3D = organicGridPointGeneration.GetPoint3DPosition();
            var points = organicGridPointGeneration.GetPoint2DPosition();
            organicGridTriangulation.LaunchOrganicGridTriangulation(organicGridCoordinates, points,
                points3D, gridBounds);
            organicGridTriangleMerger.LaunchOrganicGridTriangleMerger(
                organicGridTriangulation.GetFinalTriangles2DPosition()
                , points3D, points, gridBounds,
                organicGridCoordinates.StartPosition);

            organicGridSubdivider.LaunchOrganicGridSubdivider(organicGridTriangleMerger.GetFinalQuads(),
                organicGridTriangleMerger.GetFinalTriangles(), points, gridBounds, organicGridCoordinates.StartPosition,
                organicGridCoordinates);

            finalQuads = organicGridSubdivider.GetFinalQuads();
            finalQuadsID = organicGridSubdivider.GetFinalQuadID();
            finalPoints3D = organicGridSubdivider.GetFinalPoints3D();
        }

        public OrganicGridCoordinates GetOrganicGridCoordinates()
        {
            return organicGridCoordinates;
        }
        
        public QuadID[] GetFinalQuadID()
        {
            return finalQuadsID;
        }
        
        public Quad2DPosition[] GetFinalQuads()
        {
            return finalQuads.ToArray();
        }

        public Vector3[] GetFinalPoints3D()
        {
            return finalPoints3D;
        }

        private void CreateBaseGridObjectPrefab()
        {
            GameObject gridObject = Instantiate(baseGridObjectPrefab,
                organicGridCoordinates.StartPosition + new Vector3(0, -1, 0), quaternion.identity, transform);
            gridObject.transform.SetGlobalScale(
                new Coordinate[]
                {
                    new Coordinate(CoordinateType.X, organicGridCoordinates.GridRect.size.x + 3),
                    new Coordinate(CoordinateType.Y, 0),
                    new Coordinate(CoordinateType.Z, organicGridCoordinates.GridRect.size.y + 3)
                });
        }
    }
}