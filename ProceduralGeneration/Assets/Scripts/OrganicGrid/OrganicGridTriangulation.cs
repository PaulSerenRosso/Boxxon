using Triangulation;
using UnityEngine;

namespace OrganicGrid
{
public class OrganicGridTriangulation : MonoBehaviour
{
    [SerializeField] private float superTriangleBaseEdgeOffset = 5;
   public void LaunchOrganicGridTriangulation(Rect organicGridRect, Vector2[] points)
   {
       BowyerWatson bowyerWatson = new BowyerWatson(organicGridRect, superTriangleBaseEdgeOffset, points);
   }
}
}
