using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using MeshGenerator;
using UnityEngine;

namespace MergerTrianglesInQuadsHelper
{
    public class MergerTrianglesInQuadsObjectFactory : MonoBehaviour
    {
        [SerializeField]
        private int innerloopBatchCountForQuadGridJob;

        [SerializeField] private int innerloopBatchCountForTriangleGridJob;
        [SerializeField] private GameObject triangleMergerQuadGridPrefab;
        [SerializeField] private GameObject triangleMergerTriangleGridPrefab;

     
        private TriangleID[] currentTrianglesID;

        public void CreateTriangleMergerInQuadsObject(Vector3[] _points3D, Bounds _gridBounds, Vector3 _offset,
            Quad2DPosition[] _finalQuads, Triangle2DPosition[] _finalTriangles, Vector2[] _points)
        {
            QuadID[] finalQuadsId = MeshGeneratorHelper.GetQuadsID(_finalQuads, _points);
          
            TriangleID[] _finalTrianglesID = MeshGeneratorHelper.GetTrianglesID(_finalTriangles, _points);

            Mesh quadGridMesh = MeshGeneratorHelper.GenerateQuadGridMesh(finalQuadsId, _points3D, _gridBounds,
                innerloopBatchCountForQuadGridJob);
            Mesh triangleGridMesh =
                MeshGeneratorHelper.GenerateTriangleGridMesh(_finalTrianglesID, _points3D, _gridBounds,
                    innerloopBatchCountForTriangleGridJob);

            GameObject triangleMergerTriangleGridObject = Instantiate(triangleMergerTriangleGridPrefab,
                _gridBounds.center + _offset,
                Quaternion.identity, transform);
            triangleMergerTriangleGridObject.GetComponent<MeshFilter>().mesh = triangleGridMesh;

            GameObject triangleMergerQuadGridObject =
                Instantiate(triangleMergerQuadGridPrefab, _gridBounds.center + _offset, Quaternion.identity, transform);
            triangleMergerQuadGridObject.GetComponent<MeshFilter>().mesh = quadGridMesh;
        }
    }
}