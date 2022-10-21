using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeometryHelpers
{
    [Serializable]
    public class Segment
    {
        public Segment(Vector2 _pointA, Vector2 _pointB)
        {
            Points = new[] { _pointA, _pointB };
        }

        public Vector2[] Points;
        
    }
}