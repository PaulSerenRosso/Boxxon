using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using MeshGenerator;
using PlasticGui.WorkspaceWindow;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class OrganicGridTriangleMerger : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] private float maxTriangleToMergeCountInPercentage;

    private int maxTriangleToMergeCount = 0;

    private List<Quad> finalQuads;
    private List<Triangle2DPosition> finalTriangles;
    private List<QuadID> finalQuadsId;

    public void LaunchOrganicGridTriangleMerger(List<Triangle2DPosition> _triangles, TriangleID[] _trianglesId, Vector3[] _points3D, Bounds _gridBounds)
    {
        MergeTrianglesInQuads(_triangles);
        
    }

    private void MergeTrianglesInQuads(List<Triangle2DPosition> _triangles)
    {
        maxTriangleToMergeCount = (int) (maxTriangleToMergeCountInPercentage / 100 * _triangles.Count);
        int currentTriangleToMergeCount = 0;
        while (currentTriangleToMergeCount != maxTriangleToMergeCount && _triangles.Count != 0)
        {
            int randIndex = Random.Range(0, _triangles.Count - 1);
            for (int i = 0; i < _triangles.Count; i++)
            {
                List<Vector2> sharedVertices = _triangles[i].GetSharedVertices(_triangles[randIndex]);
                if (sharedVertices.Count == 2)
                {
                    finalQuads.Add(_triangles[i].CreateQuadWithTwoTriangle(_triangles[randIndex],
                        new Segment(sharedVertices[0], sharedVertices[1])));
                    _triangles.RemoveAt(i);
                    _triangles.RemoveAt(randIndex - 1);
                    currentTriangleToMergeCount++;
                    break;
                }
            }

            finalTriangles.Add(_triangles[randIndex]);
            _triangles.RemoveAt(randIndex);
        }
    }
}