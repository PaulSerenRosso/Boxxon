using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using MeshGenerator;

using SubdividerInQuadsHelper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrganicGrid
{
    
public class OrganicGridSubdivider : MonoBehaviour
{
    [SerializeField]
    private List<Quad2DPosition> finalQuads = new List<Quad2DPosition>();
    [SerializeField]
    private List<Vector2> finalPoints = new List<Vector2>();

    [SerializeField]
    private SubdividerInQuadsObjectFactory subdividerInQuadsObjectFactory;

    Color[] baseColors = new Color[]
    {
        Color.black, Color.blue, Color.cyan, Color.green, Color.red, Color.white, Color.yellow, Color.magenta,
        Color.grey
    };
    private Color[] colors;

    [SerializeField] private bool[] quadIsVisible;
    [SerializeField] private bool drawGizmos = false;
    public void LaunchOrganicGridSubdivider(Quad2DPosition[] _quads
        ,Triangle2DPosition[] _triangles,
        Vector2[] _points, Bounds _gridBounds, Vector3 _offset, OrganicGridCoordinates _organicGridCoordinates)
    {
        SetUp(_points);

        SubdivideQuads(_quads, _triangles);
        colors = new Color[finalQuads.Count];
        quadIsVisible = new bool[finalQuads.Count];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = baseColors[Random.Range(0, baseColors.Length)];
            quadIsVisible[i] = false;
        }
        Vector3[] points3D = ConvertPointsTo3DPoints(_organicGridCoordinates);
        subdividerInQuadsObjectFactory.CreateSubdividerInQuadsObject(points3D, _gridBounds, _offset, finalQuads.ToArray(), finalPoints.ToArray());
    }
    
    private Vector3[] ConvertPointsTo3DPoints(
        OrganicGridCoordinates _organicGridCoordinates)
    {
        Vector3[] points3D = new Vector3[finalPoints.Count];
        Vector2 gridRectSize = _organicGridCoordinates.GridRect.size;
        for (int i = 0; i < finalPoints.Count; i++)
        {
            points3D[i] =
                finalPoints[i].ConvertTo3dSpace(CoordinateType.X, CoordinateType.Z,
                    -new Vector3(gridRectSize.x, 0,
                        gridRectSize.y) / 2);
        }

        return points3D;
    }

    private void SubdivideQuads(Quad2DPosition[] _quads, Triangle2DPosition[] _triangles)
    {
        for (int i = 0; i < _quads.Length; i++)
        {
            Vector2 center = GeometryHelper.GetPolygonCenter(_quads[i].Vertices);
            List<Vector2> midEdgePoints = GeometryHelper.GetMidEdgePoints(_quads[i].Vertices);
            Quad2DPosition[] subQuads = SubdividerInQuads.SubdividePolygonInQuads(_quads[i].Vertices, center, midEdgePoints);
            for (int j = 0; j < midEdgePoints.Count; j++)
            {  
                finalPoints.Add(midEdgePoints[j]);
            }
            finalPoints.Add(center);
            for (int j = 0; j < subQuads.Length; j++)
            {
                finalQuads.Add(subQuads[j]);
            }
        }

        for (int i = 0; i < _triangles.Length; i++)
        {
            Vector2 center = GeometryHelper.GetPolygonCenter(_triangles[i].Vertices);
            List<Vector2> midEdgePoints = GeometryHelper.GetMidEdgePoints(_triangles[i].Vertices);
            Quad2DPosition[] subQuads = SubdividerInQuads.SubdividePolygonInQuads(_triangles[i].Vertices, center, midEdgePoints);
            for (int j = 0; j < midEdgePoints.Count; j++)
            {  
                finalPoints.Add(midEdgePoints[j]);
            }
            finalPoints.Add(center);
            
            for (int j = 0; j < subQuads.Length; j++)
            {
                finalQuads.Add(subQuads[j]);
            }
        }
    }

    private void SetUp(Vector2[] _points)
    {
        finalQuads.Clear();
        finalPoints = new List<Vector2>(_points);
    }

    private void OnDrawGizmosSelected()
    {
        if (drawGizmos && enabled)
            {
                if (finalQuads != null)
                {
                    for (int i = 0; i < finalQuads.Count; i++)
                    {
                        if (quadIsVisible[i])
                        {
                            Gizmos.color = colors[i];
                            Gizmos.DrawLine(finalQuads[i].Vertices[0], finalQuads[i].Vertices[1]);
                            Gizmos.DrawLine(finalQuads[i].Vertices[1], finalQuads[i].Vertices[2]);
                            Gizmos.DrawLine(finalQuads[i].Vertices[2], finalQuads[i].Vertices[3]);
                            Gizmos.DrawLine(finalQuads[i].Vertices[3], finalQuads[i].Vertices[0]);
                        }
                    }
                }
            }
        }
    }
}



