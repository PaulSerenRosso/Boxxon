using System.Collections;
using System.Collections.Generic;
using ProceduralMeshes;
using UnityEngine;

public class TriangleMeshGenerator : IMeshGenerator
{
    public Bounds Bounds { get; }
    public int VertexCount { get; }
    public int IndexCount { get; }
    public int JobLength { get; }
    public int Resolution { get; set; }
    public void Execute(int i, MeshJobTrianglesAndVertices _trianglesAndVertices)
    {
        throw new System.NotImplementedException();
    }
}
