using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MergerTrianglesInQuadsHelper
{
public class MergerTrianglesInQuads
{
    private Triangle2DPosition[] inputTriangles;
    private Vector2[] points;
    private List<Triangle2DPosition> finalTriangles = new List<Triangle2DPosition>();
    private List<Quad> finalQuads = new List<Quad>();
    private float maxTriangleToMergeCountInPercentage;
    private float minAngleForQuad;
    private float maxAngleForQuad;
    private int maxTriangleToMergeCount;
    public MergerTrianglesInQuads(Triangle2DPosition[] _triangles, Vector2[] _points, float _maxTriangleToMergeCountInPercentage,
        float _minAngleForQuad, float _maxAngleForQuad )
    {
        points = _points;
        inputTriangles = _triangles;
        maxAngleForQuad = _maxAngleForQuad;
        minAngleForQuad = _minAngleForQuad;
        maxTriangleToMergeCountInPercentage = _maxTriangleToMergeCountInPercentage;
        maxTriangleToMergeCount = (int)(maxTriangleToMergeCountInPercentage / 100 * inputTriangles.Length);
    }

    public void MergeTrianglesInQuads()
    {
      
        List<Triangle2DPosition> availableTriangles =  new List<Triangle2DPosition>(inputTriangles);
        int currentTriangleToMergeCount = 0;
        while (currentTriangleToMergeCount != maxTriangleToMergeCount &&  availableTriangles.Count != 0)
        {
            int randIndex = Random.Range(0,  availableTriangles.Count);
            bool findQuad = false;
            for (int i = 0; i <  availableTriangles.Count; i++)
            {
                if (i == randIndex)
                    continue;

                var checkIfTwoTriangleCanMerge = CheckIfTwoTriangleCanMerge(availableTriangles, randIndex, i);
                if(checkIfTwoTriangleCanMerge.twoTriangleCanMerge)
            {
                finalQuads.Add(checkIfTwoTriangleCanMerge.candidateQuad);
                            availableTriangles.RemoveAt(i);
                            if (i < randIndex)
                                availableTriangles.RemoveAt(randIndex - 1);
                            else
                            {
                                availableTriangles.RemoveAt(randIndex );
                            }
                            currentTriangleToMergeCount++;
                            findQuad = true;
                            break;
                        }
            }
            if (!findQuad)
            {
                finalTriangles.Add( availableTriangles[randIndex]);
                availableTriangles.RemoveAt(randIndex);
            }
        }

        for (int i = 0; i < availableTriangles.Count; i++)
        {
            finalTriangles.Add( availableTriangles[i]);
        }
    }
    
    private (bool twoTriangleCanMerge,Quad candidateQuad) CheckIfTwoTriangleCanMerge(List<Triangle2DPosition> _availableTriangles, int _randIndex, int _i)
    {
        List<Vector2> sharedVertices = _availableTriangles[_i].GetSharedVertices(_availableTriangles[_randIndex]);
        Quad candidateQuad;
        if (sharedVertices.Count == 2)
        {
            var communEdge = new Segment(sharedVertices[0], sharedVertices[1]);
            candidateQuad = _availableTriangles[_i]
                .CreateQuadWithTwoTriangle(_availableTriangles[_randIndex], communEdge);
            if (GeometryHelper.CheckIfPolygonIsConvex(candidateQuad.Vertices))
            {
                if (GeometryHelper.CheckIfPolygonConvexHasAllItsAnglesClamped(candidateQuad.Vertices,
                        minAngleForQuad, maxAngleForQuad))
                {
                    return (true,candidateQuad);
                }
            }
        }
        return (false, candidateQuad);
    }
}
}
