using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeometryHelpers;
using MergerTrianglesInQuadsHelper;
using MeshGenerator;
using PlasticGui.WorkspaceWindow;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace OrganicGrid
{


    public class OrganicGridTriangleMerger : MonoBehaviour
    {
        [Header("Parameters For Triangle Merger")] [Range(0, 100)] [SerializeField]
        private float maxTriangleToMergeCountInPercentage;

        [SerializeField] private float minAngleForQuad;
        [SerializeField] private float maxAngleForQuad;

        private int maxTriangleToMergeCount = 0;

        [SerializeField] private MergerTrianglesInQuadsObjectFactory mergerTrianglesInQuadsObjectFactory;

        private Quad2DPosition[] finalQuads ;
        private Triangle2DPosition[] finalTriangles;

        [Header("Parameters For Triangle Merger Gizmos ")] [SerializeField]
        private bool drawGizmos;

        Color[] baseColors = new Color[]
        {
            Color.black, Color.blue, Color.cyan, Color.green, Color.red, Color.white, Color.yellow, Color.magenta,
            Color.grey
        };
        private Color[] colors;

        [SerializeField] private bool[] quadIsVisible;


        public Quad2DPosition[] GetFinalQuads()
        {
            return finalQuads;
        }

        public Triangle2DPosition[] GetFinalTriangles()
        {
            return finalTriangles;
        }
        public void LaunchOrganicGridTriangleMerger(Triangle2DPosition[] _triangles,
            Vector3[] _points3D, Vector2[] _points, Bounds _gridBounds, Vector3 _offset)
        {
            MergerTrianglesInQuads mergerTrianglesInQuads = new MergerTrianglesInQuads(_triangles, _points,
                maxTriangleToMergeCountInPercentage, minAngleForQuad, maxAngleForQuad);
            var mergerTrianglesInQuadsResult = mergerTrianglesInQuads.MergeTrianglesInQuads();
            finalQuads = mergerTrianglesInQuadsResult.finalQuads;
            finalTriangles = mergerTrianglesInQuadsResult.finalTriangles;
            colors = new Color[mergerTrianglesInQuadsResult.finalQuads.Length];
            quadIsVisible = new bool[mergerTrianglesInQuadsResult.finalQuads.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = baseColors[Random.Range(0, baseColors.Length)];
                quadIsVisible[i] = false;
            }
            
           mergerTrianglesInQuadsObjectFactory.CreateTriangleMergerInQuadsObject(_points3D, _gridBounds, _offset,
                finalQuads, finalTriangles, _points);
                
        }


        private void OnDrawGizmosSelected()
        {
            if (drawGizmos && enabled)
            {
                if (finalQuads != null)
                {
                    for (int i = 0; i < finalQuads.Length; i++)
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