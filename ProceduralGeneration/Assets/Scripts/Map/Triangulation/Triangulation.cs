using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangulation
{
    private Rect rect;

    private float superTriangleBaseEdgeOffset;

    private Vector2[] points;
    public Triangulation(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points)
    {
        rect = _rect;
        superTriangleBaseEdgeOffset = _superTriangleBaseEdgeOffset;
        points = _points;
    }
    
    public void LaunchTriangulation()
   {
       MathHelper.GetTriangleWitchInscribesRect(rect, superTriangleBaseEdgeOffset); 
   }
}
