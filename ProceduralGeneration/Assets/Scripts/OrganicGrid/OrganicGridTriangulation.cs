using System;
using GeometryHelpers;
using JetBrains.Annotations;
using MeshGenerator;
using Triangulation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrganicGrid
{
    public class OrganicGridTriangulation : MonoBehaviour
    {
        [SerializeField] private float superTriangleBaseEdgeOffset = 5;
        [SerializeField] private ObjectTriangulation objectTriangulation;
        private BowyerWatsonWithTriangleID bowyerWatson;
        private OrganicGridCoordinates organicGridCoordinates;
        [SerializeField] private float maxAngleForTriangle;
        private Triangle2DPosition[] finalTriangles;
        
        public void LaunchOrganicGridTriangulation(OrganicGridCoordinates _organicGridCoordinates, Vector2[] _points,
            Vector3[] _3Dpoints)
        {
            organicGridCoordinates = _organicGridCoordinates;
            bowyerWatson = new BowyerWatsonWithTriangleID(_organicGridCoordinates.GridRect, superTriangleBaseEdgeOffset,
                _points, maxAngleForTriangle);
            finalTriangles = bowyerWatson.Triangulate();
            TriangleID[] trianglesId = bowyerWatson.GetTriangleID();
            objectTriangulation.LaunchObjectTriangulation(trianglesId, _3Dpoints,
                new Bounds(
                    _organicGridCoordinates.GridRect.center.ConvertTo3dSpace(CoordinateType.X, CoordinateType.Z,
                        Vector2.zero),
                    new Vector3(_organicGridCoordinates.GridRect.size.x, 2, _organicGridCoordinates.GridRect.size.y)));
        }
    }
}