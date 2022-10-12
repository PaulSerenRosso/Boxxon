﻿using System.Runtime.CompilerServices;
using ProceduralMeshes.Streams;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshes
{
    public struct MeshJobTrianglesAndVertices
    {
        [NativeDisableContainerSafetyRestriction]
        NativeArray<Vertex> vertices;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<TriangleUInt16> triangles;

        public void Setup(Mesh.MeshData _meshData)
        {
            vertices = _meshData.GetVertexData<Vertex>();
            triangles = _meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex (int _index, Vertex _vertex) => vertices[_index] = new Vertex() {
            Position = _vertex.Position,
            Normal = _vertex.Normal,
            Tangent = _vertex.Tangent,
            TexCoord0 = _vertex.TexCoord0
        };

        public void SetTriangle (int _index, int3 _triangle) => triangles[_index] = _triangle;
    }
}