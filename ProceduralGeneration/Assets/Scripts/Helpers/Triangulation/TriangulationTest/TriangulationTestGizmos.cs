using System;
using GeometryHelpers;
using JetBrains.Annotations;
using MeshGenerator;
using PointGenerator;
using Triangulation;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace OrganicGrid
{
    public class TriangulationTestGizmos : MonoBehaviour
    {
        [Header("Set in Editor")] [SerializeField]
        private Rect gridRect;

        [SerializeField] private Vector2[] points ;

        [SerializeField] private float superTriangleBaseEdgeOffset = 5;
        [SerializeField] private float maxAngleForTriangle;

        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [Header("Set in Playing")] 
        [SerializeField] private TriangulationTest[] tests;
        [SerializeField] private Circle[] circlesOfFinalTriangles;
        [SerializeField] private bool[] drawFinalTriangles;
        [SerializeField] private bool drawSuperTriangle;
        [SerializeField] private bool[] seeCircum;
        
        private Triangle2DPosition[] finalTriangles;
        private BowyerWatsonTest bowyerWatson;
        private Color[] colors;

        
        private void Start()
        {
           PoissonDiskSampling generator = new PoissonDiskSampling(gridRect.size,minDistance, maxDistance );
           points = generator.GeneratePoints();
            bowyerWatson = new BowyerWatsonTest(gridRect, superTriangleBaseEdgeOffset,
                points, maxAngleForTriangle);
            finalTriangles = bowyerWatson.Triangulate();

            Color[] baseColors = new Color[]
            {
                Color.black, Color.blue, Color.cyan, Color.green, Color.red, Color.white, Color.yellow, Color.magenta,
                Color.grey
            };
            colors = new Color[finalTriangles.Length];
            seeCircum = new bool[finalTriangles.Length];
            drawFinalTriangles = new bool[finalTriangles.Length];
            circlesOfFinalTriangles = new Circle[finalTriangles.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                circlesOfFinalTriangles[i] = finalTriangles[i].GetTriangleCircumCircle();
                seeCircum[i] = false;
                drawFinalTriangles[i] = false;
                colors[i] = baseColors[Random.Range(0, baseColors.Length - 1)];
            }

            tests = bowyerWatson.Tests;
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying && enabled)
            {
                DrawSuperTriangle();

               
                DrawFinalTriangles();

                DrawEachIterationOfBowyerWatson();
            }
        }
        private void DrawSuperTriangle()
        {
            if (drawSuperTriangle)
            {
                Gizmos.color = Color.red;
                Triangle2DPosition superTriangle = bowyerWatson.superTriangle2DPosition;
                Circle circleOfSuperTriangle = superTriangle.GetTriangleCircumCircle();

                Gizmos.DrawLine(superTriangle.Vertices[0],
                    superTriangle.Vertices[1]
                );
                Gizmos.DrawLine(superTriangle.Vertices[1],
                    superTriangle.Vertices[2]
                );
                Gizmos.DrawLine(superTriangle.Vertices[0],
                    superTriangle.Vertices[2]
                );
                Gizmos.DrawWireSphere(circleOfSuperTriangle.center, circleOfSuperTriangle.radius);
            }
        }

        private void DrawEachIterationOfBowyerWatson()
        {
            var bowyerWatsonTests = tests;
            for (int i = 0; i < bowyerWatsonTests.Length; i++)
            {
                var bowyerWatsonTest = bowyerWatsonTests[i];
                DrawCurrentPoint(bowyerWatsonTest);
                DrawCurrentTriangles(bowyerWatsonTest);
                DrawTrianglesChoosen(bowyerWatsonTest);
                DrawTriangleWhichContainCurrentPoint(bowyerWatsonTest);
                DrawFilteredTrianglesChoosen(bowyerWatsonTest);
                DrawPolygon(bowyerWatsonTest);
                
                DrawTrianglesWithoutTrianglesChoosen(bowyerWatsonTest);
                DrawNewTriangles(bowyerWatsonTest);

            }
        }

        private void DrawFilteredTrianglesChoosen(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.filteredTrianglesChoosenAreVisible)
            {
                Gizmos.color = Color.cyan;
                for (int j = 0; j < bowyerWatsonTest.filteredTrianglesChoosen.Count; j++)
                {
               
                    Gizmos.DrawLine(bowyerWatsonTest.filteredTrianglesChoosen[j].Vertices[0],
                        bowyerWatsonTest.filteredTrianglesChoosen[j].Vertices[1]);
                    Gizmos.DrawLine(bowyerWatsonTest.filteredTrianglesChoosen[j].Vertices[1],
                        bowyerWatsonTest.filteredTrianglesChoosen[j].Vertices[2]);
                    Gizmos.DrawLine(bowyerWatsonTest.filteredTrianglesChoosen[j].Vertices[2],
                        bowyerWatsonTest.filteredTrianglesChoosen[j].Vertices[0]);
                }
            }
        }
       

        void DrawTriangleWhichContainCurrentPoint(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.triangleWhichContainCurrentPointIsVisible)
            {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(bowyerWatsonTest.triangleWhichContainCurrentPoint.Vertices[0],
                bowyerWatsonTest.triangleWhichContainCurrentPoint.Vertices[1]);
            Gizmos.DrawLine(bowyerWatsonTest.triangleWhichContainCurrentPoint.Vertices[1],
                bowyerWatsonTest.triangleWhichContainCurrentPoint.Vertices[2]);
            Gizmos.DrawLine(bowyerWatsonTest.triangleWhichContainCurrentPoint.Vertices[2],
                bowyerWatsonTest.triangleWhichContainCurrentPoint.Vertices[0]);
                
            }
        }
        private void DrawTrianglesWithoutTrianglesChoosen(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.trianglesWithoutTrianglesChoosenAreVisible)
            {
                for (int j = 0; j < bowyerWatsonTest.trianglesWithoutTrianglesChoosen.Count; j++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(bowyerWatsonTest.trianglesWithoutTrianglesChoosen[j].Vertices[0],
                        bowyerWatsonTest.trianglesWithoutTrianglesChoosen[j].Vertices[1]);
                    Gizmos.DrawLine(bowyerWatsonTest.trianglesWithoutTrianglesChoosen[j].Vertices[1],
                        bowyerWatsonTest.trianglesWithoutTrianglesChoosen[j].Vertices[2]);
                    Gizmos.DrawLine(bowyerWatsonTest.trianglesWithoutTrianglesChoosen[j].Vertices[2],
                        bowyerWatsonTest.trianglesWithoutTrianglesChoosen[j].Vertices[0]);
                }
            }
        }

        private void DrawTrianglesChoosen(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.trianglesChoosenAreVisible)
            {
                foreach (var triangleWithCircle in bowyerWatsonTest.trianglesChoosenWithCircle)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(triangleWithCircle.Key.Vertices[0], triangleWithCircle.Key.Vertices[1]);
                    Gizmos.DrawLine(triangleWithCircle.Key.Vertices[1], triangleWithCircle.Key.Vertices[2]);
                    Gizmos.DrawLine(triangleWithCircle.Key.Vertices[2], triangleWithCircle.Key.Vertices[0]);
                    Gizmos.DrawWireSphere(triangleWithCircle.Value.center, triangleWithCircle.Value.radius);
                }
            }
        }

        private void DrawNewTriangles(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.newTrianglesAreVisible)
            {
                for (int j = 0; j < bowyerWatsonTest.newTriangles.Count; j++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(bowyerWatsonTest.newTriangles[j].Vertices[0],
                        bowyerWatsonTest.newTriangles[j].Vertices[1]);
                    Gizmos.DrawLine(bowyerWatsonTest.newTriangles[j].Vertices[1],
                        bowyerWatsonTest.newTriangles[j].Vertices[2]);
                    Gizmos.DrawLine(bowyerWatsonTest.newTriangles[j].Vertices[2],
                        bowyerWatsonTest.newTriangles[j].Vertices[0]);
                }
            }
        }

        private void DrawCurrentTriangles(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.currentTriangleAreVisible)
            {
                for (int j = 0; j < bowyerWatsonTest.currentTriangles.Count; j++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(bowyerWatsonTest.currentTriangles[j].Vertices[0],
                        bowyerWatsonTest.currentTriangles[j].Vertices[1]);
                    Gizmos.DrawLine(bowyerWatsonTest.currentTriangles[j].Vertices[1],
                        bowyerWatsonTest.currentTriangles[j].Vertices[2]);
                    Gizmos.DrawLine(bowyerWatsonTest.currentTriangles[j].Vertices[2],
                        bowyerWatsonTest.currentTriangles[j].Vertices[0]);
                }
            }
        }

        private void DrawPolygon(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.polygonIsVisible)
            {
                for (int j = 0; j < bowyerWatsonTest.polygon.Count; j++)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(bowyerWatsonTest.polygon[j].Points[0], bowyerWatsonTest.polygon[j].Points[1]);
                }
            }
        }

        private void DrawCurrentPoint(TriangulationTest bowyerWatsonTest)
        {
            if (bowyerWatsonTest.pointIsVisible)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawSphere(bowyerWatsonTest.point, 0.5f);
            }
        }

        private void DrawFinalTriangles()
        {
            for (int i = 0; i < finalTriangles.Length; i++)
            {
                if (drawFinalTriangles[i])
                {
                    Gizmos.color = colors[i];

                    Gizmos.DrawLine(finalTriangles[i].Vertices[0],
                        finalTriangles[i].Vertices[1]
                    );
                    Gizmos.DrawLine(finalTriangles[i].Vertices[1],
                        finalTriangles[i].Vertices[2]
                    );
                    Gizmos.DrawLine(finalTriangles[i].Vertices[0],
                        finalTriangles[i].Vertices[2]
                    );
                }

                if (seeCircum[i])
                    Gizmos.DrawWireSphere(circlesOfFinalTriangles[i].center, circlesOfFinalTriangles[i].radius);
            }
        }

    }
}