using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using MeshGenerator;
using UnityEngine;

public class OrganicGridSubdivider : MonoBehaviour
{
    private List<QuadID> finalQuadsID = new List<QuadID>();
    private List<Quad> finalQuads = new List<Quad>();
    private List<Vector2> finalPoints = new List<Vector2>();
    public void LaunchOrganicGridTriangleSubdivider(List<Quad> _quads, QuadID[] _quadsId
        ,List<Triangle2DPosition> _triangles, TriangleID[] _trianglesId,
        Vector3[] _points3D, Vector2[] _points, Bounds _gridBounds, Vector3 _offset)
    {
        finalQuads.Clear();
        finalQuadsID.Clear();
        finalPoints = new List<Vector2>(_points);
        
        for (int i = 0; i < _quads.Count; i++)
        {
            Vector2 center = GeometryHelper.GetPolygonCenter(_quads[i].Vertices);
            Quad[] subQuads = GeometryHelper.SubdividePolygonInQuads(_quads[i].Vertices, center);
            
            finalPoints.Add(center);
            for (int j = 0; j < _quads[i].Vertices.Length ; j++)
            {
                
            }
            for (int j = 0; j < subQuads.Length; j++)
            {
              
                finalQuads.Add(subQuads[j]);
                
            }
        }

        for (int i = 0; i < _triangles.Count; i++)
        {
            Vector2 center = GeometryHelper.GetPolygonCenter(_triangles[i].Vertices);
            Quad[] subQuads = GeometryHelper.SubdividePolygonInQuads(_triangles[i].Vertices);
            finalPoints.Add(center);
            
            
        }
    }
}
