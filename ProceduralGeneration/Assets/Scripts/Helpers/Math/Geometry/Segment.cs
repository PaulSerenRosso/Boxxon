using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeometryHelpers
{
    [Serializable]
    public struct Segment
    {
        public Segment(Vector2 _pointA, Vector2 _pointB)
        {
            Points = new[] { _pointA, _pointB };
        }

        public Vector2[] Points;
        public static bool operator == (Segment a, Segment b)
        {
            int sharedVertices = 0;
            for (int i = 0; i < a.Points.Length; i++)
            {
                for (int j = 0; j < b.Points.Length; j++)
                {
                    if (a.Points[i] == b.Points[j])
                    {
                        sharedVertices++;
                    }
                }
            }

            if (sharedVertices == 2)
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(Segment a, Segment b)
        {
            return !(a == b);
        }
    }
}