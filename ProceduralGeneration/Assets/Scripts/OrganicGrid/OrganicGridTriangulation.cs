using System;
using System.Linq;
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
        [SerializeField] private TriangulationObjectFactory objectTriangulation;
        private BowyerWatson bowyerWatson;
        private OrganicGridCoordinates organicGridCoordinates;
        [SerializeField] private float maxAngleForTriangle;
        private Triangle2DPosition[] finalTriangles;
        private TriangleID[] finalTrianglesId;
        
        public void LaunchOrganicGridTriangulation(OrganicGridCoordinates _organicGridCoordinates, Vector2[] _points,
            Vector3[] _3Dpoints, Bounds _gridBounds)
        {
            organicGridCoordinates = _organicGridCoordinates;
            bowyerWatson = new BowyerWatson(_organicGridCoordinates.GridRect, superTriangleBaseEdgeOffset,
                _points, maxAngleForTriangle);
            finalTriangles = bowyerWatson.Triangulate();
            objectTriangulation.CreateObjectTriangulation(finalTriangles,_points, _3Dpoints,_gridBounds,organicGridCoordinates.StartPosition
         );
        }

      public  Triangle2DPosition[] GetFinalTriangles2DPosition()
        {
            return finalTriangles;
        }

        public TriangleID[] GetFinalTrianglesID()
        {
            return finalTrianglesId;
        }
    }
}