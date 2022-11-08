using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using MeshGenerator;
using PlasticGui.WorkspaceWindow;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class OrganicGridTriangleMerger : MonoBehaviour
{
    [Header("Parameters For Triangle Merger")]
    [Range(0, 100)] [SerializeField] private float maxTriangleToMergeCountInPercentage;
    [SerializeField] private float minAngleForQuad;
    [SerializeField] private float maxAngleForQuad;
    [SerializeField] private int innerloopBatchCountForQuadGridJob;
    [SerializeField] private int innerloopBatchCountForTriangleGridJob;
    [SerializeField] private GameObject triangleMergerTriangleGridPrefab;
    [SerializeField] private GameObject triangleMergerQuadGridPrefab;

    private int maxTriangleToMergeCount = 0;
    private List<Quad> finalQuads = new List<Quad>();
    private List<Triangle2DPosition> finalTriangles = new List<Triangle2DPosition>();
    private List<QuadID> finalQuadsId = new List<QuadID>();
    private List<TriangleID> finalTrianglesID = new List<TriangleID>();
    private List<Triangle2DPosition> triangle2DPositions;
    private List<Triangle2DPosition> currentTriangles;
    private TriangleID[] currentTrianglesID;

    [Header("Parameters For Triangle Merger Gizmos ")]
    [SerializeField] private bool drawGizmos;
    Color[] baseColors = new Color[]
    {
        Color.black, Color.blue, Color.cyan, Color.green, Color.red, Color.white, Color.yellow, Color.magenta,
        Color.grey
    };

   
    [SerializeField]
    private bool[] quadIsVisible;

    private Color[] colors;
    
   
    private Vector2[] points;
    public void LaunchOrganicGridTriangleMerger(List<Triangle2DPosition> _triangles, TriangleID[] _trianglesId,
        Vector3[] _points3D, Vector2[] _points, Bounds _gridBounds, Vector3 _offset)
    {
        SetUpTriangleMerger(_triangles, _trianglesId, _points);
        MergeTrianglesInQuads();
        colors = new Color[finalQuadsId.Count];
        quadIsVisible = new bool[finalQuadsId.Count];
        
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = baseColors[Random.Range(0, baseColors.Length)];
            quadIsVisible[i] = false;
        }
     GenerateTriangleMergerMeshes(_points3D, _gridBounds, _offset);
    }

    private void GenerateTriangleMergerMeshes(Vector3[] _points3D  ,Bounds _gridBounds, Vector3 _offset)
    {
        Mesh quadGridMesh = MeshGeneratorHelper.GenerateQuadGridMesh(finalQuadsId.ToArray(), _points3D, _gridBounds,
            innerloopBatchCountForQuadGridJob);
        Mesh triangleGridMesh =
            MeshGeneratorHelper.GenerateTriangleGridMesh(finalTrianglesID.ToArray(), _points3D, _gridBounds,
                innerloopBatchCountForTriangleGridJob);
        GameObject triangleMergerTriangleGridObject = Instantiate(triangleMergerTriangleGridPrefab, _gridBounds.center+_offset,
            Quaternion.identity, transform);
        triangleMergerTriangleGridObject.GetComponent<MeshFilter>().mesh = triangleGridMesh;

        GameObject triangleMergerQuadGridObject =
            Instantiate(triangleMergerQuadGridPrefab, _gridBounds.center+_offset, Quaternion.identity, transform);
        triangleMergerQuadGridObject.GetComponent<MeshFilter>().mesh = quadGridMesh;
    }

    private void SetUpTriangleMerger(List<Triangle2DPosition> _triangles, TriangleID[] _trianglesId, Vector2[] _points)
    {
        finalQuads.Clear();
        finalTriangles.Clear();
        finalQuadsId.Clear();
        finalTrianglesID.Clear();
        points = _points;
        
        currentTriangles = _triangles;
        currentTrianglesID = _trianglesId;
    }

    private void MergeTrianglesInQuads()
    {
        
        maxTriangleToMergeCount = (int)(maxTriangleToMergeCountInPercentage / 100 * currentTriangles.Count);
        List<Triangle2DPosition> availableTriangles =  new List<Triangle2DPosition>(currentTriangles);
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
                            int randTriangleIndexInCurrentTriangles =
                                currentTriangles.IndexOf(availableTriangles[randIndex]);
                            int triangleBeLookedIndexInCurrentTriangles =
                                currentTriangles.IndexOf(availableTriangles[i]);
                            CreateQuadId(triangleBeLookedIndexInCurrentTriangles, randTriangleIndexInCurrentTriangles,
                                checkIfTwoTriangleCanMerge.communEdge);
                            var lastQuadID = finalQuadsId[finalQuadsId.Count - 1];
                            
                            Quad finalQuad = new Quad(points[lastQuadID.A],
                                points[lastQuadID.B],
                                points[lastQuadID.C],
                                points[lastQuadID.D]);
                     
                            finalQuads.Add(finalQuad);
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
                int randTriangleIndexInCurrentTriangles =
                    currentTriangles.IndexOf(availableTriangles[randIndex]);
                finalTriangles.Add( availableTriangles[randIndex]);
                finalTrianglesID.Add(currentTrianglesID[randTriangleIndexInCurrentTriangles]);
                 availableTriangles.RemoveAt(randIndex);
            }
        }
    }

    private void CreateQuadId(int _i, int _randIndex, Segment _communEdge)
    {
        var triangleBeLookedOppositeVertexIndex = GetTriangleOppositeVertexIndex(currentTriangles[_i], _communEdge);
        int triangleBeLookedOppositeVertexID =
            currentTrianglesID[_i].SelectIDInTriangleId(triangleBeLookedOppositeVertexIndex);

        int randomTriangleOppositeVertexIndex =
            GetTriangleOppositeVertexIndex(currentTriangles[_randIndex], _communEdge);
        int randomTriangleBeLookedOppositeVertexID = currentTrianglesID[_randIndex]
            .SelectIDInTriangleId(randomTriangleOppositeVertexIndex);

        int firstPointSegmentInTriangleBeLookedIndex =
            Array.IndexOf(currentTriangles[_i].Vertices, _communEdge.Points[0]);
        int firstPointSegmentInTriangleBeLookedID =
            currentTrianglesID[_i].SelectIDInTriangleId(firstPointSegmentInTriangleBeLookedIndex);

        int secondPointSegmentInTriangleBeLookedIndex =
            Array.IndexOf(currentTriangles[_i].Vertices, _communEdge.Points[1]);
        int secondPointSegmentInTriangleBeLookedID =
            currentTrianglesID[_i].SelectIDInTriangleId(secondPointSegmentInTriangleBeLookedIndex);

        QuadID finalQuadId = new QuadID(new int4(firstPointSegmentInTriangleBeLookedID,
            randomTriangleBeLookedOppositeVertexID,
            secondPointSegmentInTriangleBeLookedID, triangleBeLookedOppositeVertexID));
        
        Vector3 pointA = points[finalQuadId.A];
        Vector3 pointD = points[finalQuadId.D];
        Vector3 pointC = points[finalQuadId.C];
        if (!GeometryHelper.IsCounterClockwise(pointA, pointD, pointC))
        {
            QuadID finalQuadIdClockwise = new QuadID(new int4(finalQuadId.A, finalQuadId.D, finalQuadId.C, finalQuadId.B));
            finalQuadsId.Add(finalQuadIdClockwise);
        }
        else
        {
        finalQuadsId.Add(finalQuadId);
            
        }
        
    }

    private  int GetTriangleOppositeVertexIndex(Triangle2DPosition _triangle, Segment communEdge)
    {
        Vector2 triangleBeLookedOppositeVertex = _triangle.GetTheOppositeVertexToTheEdge(communEdge);
        int triangleBeLookedOppositeVertexIndex =
            Array.IndexOf(_triangle.Vertices, triangleBeLookedOppositeVertex);
        return triangleBeLookedOppositeVertexIndex;
    }

    private (bool twoTriangleCanMerge, Segment communEdge) CheckIfTwoTriangleCanMerge(List<Triangle2DPosition> _availableTriangles, int _randIndex, int _i)
    {
        List<Vector2> sharedVertices = _availableTriangles[_i].GetSharedVertices(_availableTriangles[_randIndex]);
        if (sharedVertices.Count == 2)
        {
            var communEdge = new Segment(sharedVertices[0], sharedVertices[1]);
            var candidateQuad = _availableTriangles[_i]
                .CreateQuadWithTwoTriangle(_availableTriangles[_randIndex], communEdge);
            if (GeometryHelper.CheckIfPolygonIsConvex(candidateQuad.Vertices))
            {
                if (GeometryHelper.CheckIfPolygonConvexHasAllItsAnglesClamped(candidateQuad.Vertices,
                    minAngleForQuad, maxAngleForQuad))
                {
                    return (true,communEdge);
                }
            }
        }
        return (false, new Segment());
    }

    private void OnDrawGizmosSelected()
    {
        if (drawGizmos && enabled)
        {
            if (finalQuadsId.Count != 0)
            {
                
            for (int i = 0; i < finalQuadsId.Count; i++)
            {
                if (quadIsVisible[i])
                {
                Gizmos.color =colors[i];
                Gizmos.DrawLine(points[finalQuadsId[i].A], points[finalQuadsId[i].B]);
                Gizmos.DrawLine(points[finalQuadsId[i].B], points[finalQuadsId[i].C]);
                Gizmos.DrawLine(points[finalQuadsId[i].C], points[finalQuadsId[i].D]);
                Gizmos.DrawLine(points[finalQuadsId[i].D], points[finalQuadsId[i].A]);
                }
            }
                
            }
        }
    }
}