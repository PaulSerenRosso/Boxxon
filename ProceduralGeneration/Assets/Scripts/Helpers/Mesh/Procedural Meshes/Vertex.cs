using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace MeshGenerator
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public float3 Position, Normal;
        public float4 Tangent;
        public float2 TexCoord0;
    }
}