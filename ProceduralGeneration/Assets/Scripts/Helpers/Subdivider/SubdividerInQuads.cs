using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;

namespace SubdividerInQuadsHelper
{
    public static class SubdividerInQuads
    {
        public static Quad2DPosition[] SubdividePolygonInQuads(this Vector2[] _vertices)
        {
            Vector2 center = GeometryHelper.GetPolygonCenter(_vertices);
            List<Vector2> midEdgePoints = GeometryHelper.GetMidEdgePoints(_vertices);
            Quad2DPosition[] quads = new Quad2DPosition[midEdgePoints.Count];
            for (int i = 0; i < midEdgePoints.Count - 1; i++)
            {
                quads[i] = new Quad2DPosition(center, midEdgePoints[i + 1], _vertices[i],midEdgePoints[i]);
            }

            quads[midEdgePoints.Count - 1] = new Quad2DPosition(center, midEdgePoints[midEdgePoints.Count - 1],
                 _vertices[_vertices.Length - 1], midEdgePoints[0]);
            return quads;
        }

        public static Quad2DPosition[] SubdividePolygonInQuads(this Vector2[] _vertices, Vector2 _center)
        {
            List<Vector2> midEdgePoints = GeometryHelper.GetMidEdgePoints(_vertices);
            Quad2DPosition[] quads = new Quad2DPosition[midEdgePoints.Count];
            for (int i = 0; i < midEdgePoints.Count - 1; i++)
            {
                quads[i] = new Quad2DPosition(_center, midEdgePoints[i + 1],_vertices[i], midEdgePoints[i]);
            }

            quads[midEdgePoints.Count - 1] = new Quad2DPosition(_center, midEdgePoints[midEdgePoints.Count - 1],_vertices[_vertices.Length - 1], 
                midEdgePoints[0]  );
            return quads;
        }
        
        public static Quad2DPosition[] SubdividePolygonInQuads(this Vector2[] _vertices, Vector2 _center, List<Vector2> _midEdgePoints)
        {
         
            Quad2DPosition[] quads = new Quad2DPosition[_midEdgePoints.Count];
            for (int i = 0; i < _midEdgePoints.Count - 1; i++)
            {
                quads[i] = new Quad2DPosition(_center, _midEdgePoints[i + 1],_vertices[i], _midEdgePoints[i]);
            }

            quads[_midEdgePoints.Count - 1] = new Quad2DPosition(_center, _midEdgePoints[_midEdgePoints.Count - 1],_vertices[_vertices.Length - 1], 
                _midEdgePoints[0]  );
            return quads;
        }
    }
}