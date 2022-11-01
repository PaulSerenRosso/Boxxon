using System.Runtime.CompilerServices;
using GeometryHelpers;
using JobHelpers;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace MeshGenerator
{
    public struct MeshJobTrianglesAndVertices
    {
        [NativeDisableContainerSafetyRestriction]
        NativeArray<Vertex> vertices;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<TriangleID> triangles;

        [NativeDisableContainerSafetyRestriction]
        private Mesh.MeshData meshData;
        public void Setup(Mesh.MeshData _meshData)
        {
            meshData = _meshData;
            vertices = _meshData.GetVertexData<Vertex>();
            triangles = _meshData.GetIndexData<ushort>().Reinterpret<TriangleID>(2);
        //    JobHelper.Log(triangles.Length);
         //   JobHelper.Log(vertices.Length);
 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int _index, Vertex _vertex)
        {
            vertices[_index] = new Vertex()
            {
                Position = _vertex.Position,
                Normal = _vertex.Normal,
                Tangent = _vertex.Tangent,
                TexCoord0 = _vertex.TexCoord0
            };
        }

        public void SetTriangle(int _index, int3 _indices)
        {
            triangles[_index] = _indices;
        //    NativeArray<ushort> nativeArray = meshData.GetIndexData<ushort>();
          //  JobHelper.Log("settriangle"+nativeArray[_index]);
        }
            
    }
}