using System.Collections;
using System.Collections.Generic;
using MeshGenerator;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class QuadGridMeshGenerator // : IMeshGenerator
{
    /*
 // index count c'est le nombre de vertices et vertexcount c'est le nombre de point 
        [NativeDisableParallelForRestriction]
        private NativeArray<TriangleID> trianglesID;
        [NativeDisableParallelForRestriction]
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

            points = new NativeArray<float3>(_points.Length, Allocator.TempJob);
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
            vertexA.TexCoord0 = new float2(0, 0);
            vertexA.Normal = new float3(0, 1, 0);
            var indexA = i * 3;
            _trianglesAndVertices.SetVertex(indexA, vertexA);

            Vertex vertexB = new Vertex();
            var pointB = points[trianglesID[i].B];
            vertexB.Position = pointB;
            vertexB.TexCoord0 = new float2(1, 0);
            vertexB.Normal = new float3(0, 1, 0);
            
            var indexB = i * 3+1;
            _trianglesAndVertices.SetVertex(indexB, vertexB);
            
            Vertex vertexC = new Vertex();
            var pointC = points[trianglesID[i].C];
            vertexC.Position = pointC;
            vertexC.TexCoord0 = new float2(0, 1);
            vertexC.Normal = new float3(0, 1, 0);
            var indexC = i * 3+2;
            _trianglesAndVertices.SetVertex(indexC, vertexC);
            
            _trianglesAndVertices.SetTriangle(i,new int3(indexA,indexB,indexC) );
        }
    }
    */
}
