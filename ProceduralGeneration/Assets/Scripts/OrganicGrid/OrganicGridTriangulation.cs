using System;
using GeometryHelpers;
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
    public Color[] colors;
    public Circle[] circles;

    private Triangle2DPosition[] triangle2DPositions;
   public void LaunchOrganicGridTriangulation(OrganicGridCoordinates _organicGridCoordinates, Vector2[] _points, Vector3[] _3Dpoints)
   {
       organicGridCoordinates = _organicGridCoordinates;
      bowyerWatson = new  BowyerWatsonWithTriangleID(_organicGridCoordinates.GridRect, superTriangleBaseEdgeOffset, _points);
    triangle2DPositions=  bowyerWatson.Triangulate();
    
    colors = new Color[triangle2DPositions.Length];
    circles = new Circle[triangle2DPositions.Length];
    for (int i = 0; i < colors.Length; i++)
    {
        colors[i] = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
    }
        circles[0] = triangle2DPositions[0].GetTriangleCircumCircle();
    TriangleID[] trianglesId =   bowyerWatson.GetTriangleID();

     objectTriangulation.LaunchObjectTriangulation(trianglesId, _3Dpoints,
         new Bounds( _organicGridCoordinates.GridRect.center.ConvertTo3dSpace(CoordinateType.X, CoordinateType.Z, _organicGridCoordinates.StartPosition),
             new Vector3(_organicGridCoordinates.GridRect.size.x,2 , _organicGridCoordinates.GridRect.size.y)));
   }

   private void OnDrawGizmos()
   {
     Gizmos.color = Color.red;
     
     Triangle2DPosition test = bowyerWatson.superTriangle2DPosition;
     Circle circleTest = test.GetTriangleCircumCircle();
     
     Gizmos.DrawLine(test.Vertices[0], 
         test.Vertices[1]
         );
     Gizmos.DrawLine(test.Vertices[1], 
         test.Vertices[2]
     );
     Gizmos.DrawLine(test.Vertices[0], 
         test.Vertices[2]
     );
    
     Gizmos.DrawWireSphere(circleTest.center, circleTest.radius);
     
     for (int i = 0; i < test.Vertices.Length-1; i++)
     { 
     Gizmos.color = Color.yellow;
        Gizmos.DrawLine(test.Vertices[i]+(test.Vertices[i + 1] -
                                                                          test.Vertices[i]) / 2,
            circleTest.center);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(test.Vertices[i] +
                        (test.Vertices[i + 1] -
                         test.Vertices[i]) / 2,
            test.Vertices[i] +   (test.Vertices[i + 1] -
                                                                  test.Vertices[i]) / 2+Vector2.Perpendicular(
                                  (test.Vertices[i + 1] -
                                   test.Vertices[i])));

     }

     for (int i = 0; i < triangle2DPositions.Length; i++)
     {
     Gizmos.color = colors[i];
     Gizmos.DrawWireSphere(circles[i].center, circles[i].radius);
         Gizmos.DrawLine(triangle2DPositions[i].Vertices[0], 
             triangle2DPositions[i].Vertices[1]
         );
         Gizmos.DrawLine(triangle2DPositions[i].Vertices[1], 
             triangle2DPositions[i].Vertices[2]
         );
         Gizmos.DrawLine(triangle2DPositions[i].Vertices[0], 
             triangle2DPositions[i].Vertices[2]
         );
     }
   }
}
}
