using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MeshGenerator
{
    public struct TriangleGridMeshGenerator : IMeshGenerator
    {
        // index count c'est le nombre de vertices et vertexcount c'est le nombre de point 
        private NativeArray<TriangleID> trianglesID;
        private NativeArray<float3> points;

        //   const float2 firstUVCoord; 
        //   const float2 secondUVCoord; 
        //   const float2 thirdUVCoord; 
        public void SetUp(Vector3[] _points, TriangleID[] _trianglesID, Bounds _bounds)
        {
            trianglesID = new NativeArray<TriangleID>(_trianglesID.Length, Allocator.TempJob);
            for (int i = 0; i < _trianglesID.Length; i++)
            {
                trianglesID[i] = _trianglesID[i];
            }

            points = new NativeArray<float3>(points.Length, Allocator.TempJob);
            for (int i = 0; i < _points.Length; i++)
            {
                points[i] = _points[i];
            }
            bounds = _bounds;
        }


        private Bounds bounds; 

        public Bounds Bounds
        {
            get => bounds;
        }

        public int VertexCount
        {
            get => trianglesID.Length * 3;
        }

        public int IndexCount
        {
            get => trianglesID.Length * 3;
        }

        public int JobLength
        {
            get => trianglesID.Length;
        }

        public void Execute(int i, MeshJobTrianglesAndVertices _trianglesAndVertices)
        {
            Vertex vertexA = new Vertex();
            var pointA = points[trianglesID[i].A];
            vertexA.Position = pointA;
            var indexA = i * 3;
            _trianglesAndVertices.SetVertex(indexA, vertexA);

            Vertex vertexB = new Vertex();
            var pointB = points[trianglesID[i].B];
            vertexA.Position = pointB;
            var indexB = i * 3+1;
            _trianglesAndVertices.SetVertex(indexB, vertexB);
            
            Vertex vertexC = new Vertex();
            var pointC = points[trianglesID[i].C];
            vertexA.Position = pointC;
            var indexC = i * 3+2;
            
            _trianglesAndVertices.SetVertex(indexC, vertexC);
            _trianglesAndVertices.SetTriangle(i,new int3( indexA, indexB, indexC));
        }
    }
}