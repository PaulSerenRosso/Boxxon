using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MeshUtils 
{
    public static void RecalBoundsNormals(Mesh mesh)
    {
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
