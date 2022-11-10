using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;

namespace MergerTrianglesInQuadsHelper
{
    public class MergerTrianglesInQuadsID : MergerTrianglesInQuads

    {
        public MergerTrianglesInQuadsID(
            Triangle2DPosition[] _triangles, Vector2[] _points, float _maxTriangleToMergeCountInPercentage, 
            float _minAngleForQuad, float _maxAngleForQuad) : base(_triangles, _points, _maxTriangleToMergeCountInPercentage,
            _minAngleForQuad, _maxAngleForQuad)
        {
            
        }
    }
}
