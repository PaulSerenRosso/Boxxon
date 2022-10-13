using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using Unity.Collections;
using UnityEngine;

namespace MeshGenerator
{


    public struct TriangleMeshGenerator : IMeshGenerator
    {
        public Bounds Bounds { get; }
        public int VertexCount { get; }
        public int IndexCount { get; }
        public int JobLength { get => trianglesPosition.Length; }
        
        private NativeArray<Triangle2DPositionForJob> trianglesPosition; 
 
        public void SetUp(Triangle2DPosition[] _trianglesPosition)
        {
            trianglesPosition = new NativeArray<Triangle2DPositionForJob>(_trianglesPosition.Length, Allocator.TempJob);
            for (int i = 0; i < _trianglesPosition.Length; i++)
            {
                trianglesPosition[i] = _trianglesPosition[i];
            }
            
        }
        
       
        public void Execute(int i, MeshJobTrianglesAndVertices _trianglesAndVertices)
        {

        }
    }
}
