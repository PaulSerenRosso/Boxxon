using Triangulation;
using UnityEngine;

namespace OrganicGrid
{
public class OrganicGridTriangulation : MonoBehaviour
{
    [SerializeField] private float superTriangleBaseEdgeOffset = 5;
   public void LaunchOrganicGridTriangulation(OrganicGridCoordinates organicGridCoordinates, Vector2[] points)
   {
       BowyerWatson bowyerWatson = new BowyerWatson(organicGridCoordinates.GridRect, superTriangleBaseEdgeOffset, points);
   }
}
}
