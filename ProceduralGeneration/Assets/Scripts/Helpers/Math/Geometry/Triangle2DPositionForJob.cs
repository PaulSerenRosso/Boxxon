using Unity.Mathematics;

namespace GeometryHelpers
{
    public struct Triangle2DPositionForJob
    {
       public float2 A; 
       public float2 B; 
       public float2 C;

       public Triangle2DPositionForJob(float2 _a, float2 _b, float2 _c)
       {
           A = _a;
           B = _b;
           C = _c;
       }
       public static implicit operator Triangle2DPositionForJob(Triangle2DPosition triangle2DPosition) => new Triangle2DPositionForJob()
       {
           A = triangle2DPosition.Vertices[0], B = triangle2DPosition.Vertices[1], C = triangle2DPosition.Vertices[2]
       };
    }
}