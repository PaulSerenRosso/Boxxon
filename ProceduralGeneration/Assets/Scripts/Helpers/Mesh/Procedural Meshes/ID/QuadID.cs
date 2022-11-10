using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using MeshGenerator;
using Unity.Mathematics;
using UnityEngine;


public struct QuadID
{
    public TriangleID FirstTriangle;
    public TriangleID SecondTriangle;
    public int4 Vertices;

    public QuadID(TriangleID _firstTriangle, TriangleID _secondTriangle, int4 _vertices)
    {
        FirstTriangle = _firstTriangle;
        SecondTriangle = _secondTriangle;
        Vertices = _vertices;
    }

}
