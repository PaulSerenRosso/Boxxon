using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using Unity.Mathematics;
using UnityEngine;


public struct QuadID 
{
    public ushort A, B, C, D;

    public QuadID(int4 _ids)
    {
        A = (ushort) _ids.x;
        B = (ushort) _ids.y;
        C = (ushort) _ids.z;
        D = (ushort) _ids.w;
    }
}
