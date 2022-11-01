using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;

public class CircumcircleTriangleTest : MonoBehaviour
{
    [SerializeField]
    private Vector2 vertexA;
    [SerializeField]
    private Vector2 vertexB;
    [SerializeField]
    private Vector2 vertexC;
    private Triangle2DPosition triangle;

    private Circle circle;
    private Vector2[] midPoint = new Vector2[2];
   
    private void OnValidate()
    {
        triangle = new Triangle2DPosition(vertexA, vertexB, vertexC);
     circle = triangle.GetTriangleCircumCircle();
     for (int i = 0; i < triangle.Vertices.Length-1; i++)
     {
         Vector2 dir = triangle.Vertices[i + 1] -
                       triangle.Vertices[i];
         midPoint[i] = triangle.Vertices[i] + (dir) / 2;
     }
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(triangle.Vertices[0], 
            triangle.Vertices[1]
        );
        Gizmos.DrawLine(triangle.Vertices[1], 
            triangle.Vertices[2]
        );
        Gizmos.DrawLine(triangle.Vertices[0], 
            triangle.Vertices[2]
        );
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(circle.center, circle.radius);
        for (int i = 0; i <triangle.Vertices.Length-1 ; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(midPoint[i],circle.center);
        }
 
   }
    
}
