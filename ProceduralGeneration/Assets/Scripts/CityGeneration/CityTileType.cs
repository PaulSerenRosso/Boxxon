using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityGeneratorHelper
{
    [Serializable]
    public class CityTileType
    {
        public Mesh MeshType;
        public float OffsetMesh;
        public float RatioAreaYSize;
        public Vector3[] Bounds2DVertices;
    }
}
