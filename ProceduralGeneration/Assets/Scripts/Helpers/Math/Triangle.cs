using System;
using Unity.Mathematics;


[Serializable]
public struct Triangle
{
   public float3 vert0;
   public float3 vert1;
   public float3 vert2;
   
   public Triangle(float3 _vert0, float3 _vert1, float3 _vert2)
   {
      vert0 = _vert0;
      vert1 = _vert1;
      vert2 = _vert2;
   }
}