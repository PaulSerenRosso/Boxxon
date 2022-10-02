using System.Collections.Generic;
using UnityEngine;

namespace Triangulation
{
public class BowyerWatson
{
    private Rect rect ;

    private float superTriangleBaseEdgeOffset;

    private Vector2[] points;
    public BowyerWatson(Rect _rect, float _superTriangleBaseEdgeOffset, Vector2[] _points)
    {
        rect = _rect;
        superTriangleBaseEdgeOffset = _superTriangleBaseEdgeOffset;
        points = _points;
    }
    
    public Triangle[] Triangulate()
    {
        Triangle superTriangle = MathHelper.GetTriangleWitchInscribesRect(rect, superTriangleBaseEdgeOffset);
        Dictionary<Triangle, Circle> trianglesWithCircumCircle = new Dictionary<Triangle, Circle>();
        trianglesWithCircumCircle.Add(superTriangle, superTriangle.GetTriangleCircumCircle());
        for (int i = 0; i < points.Length; i++)
        {
            foreach (var triangleWithCircumCircle in trianglesWithCircumCircle)
            {
                if ((points[i] - triangleWithCircumCircle.Value.center).sqrMagnitude <=
                    triangleWithCircumCircle.Value.radius * triangleWithCircumCircle.Value.radius)
                {
                    
                }
            }
        }
        return new Triangle[1];
    }
}
}
