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
       MathHelper.GetTriangleWitchInscribesRect(rect, superTriangleBaseEdgeOffset);
       return new Triangle[2];
    }
}
}
