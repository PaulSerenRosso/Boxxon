using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using MeshGenerator;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TriangulationTest
{
    public bool pointIsVisible;
    public bool currentTriangleAreVisible;
    public bool trianglesChoosenAreVisible; 
    public bool polygonIsVisible;
    public bool trianglesWithoutTrianglesChoosenAreVisible;
    public bool newTrianglesAreVisible;
    [HideInInspector] public Vector2 point;
    [HideInInspector] public List<Triangle2DPosition> currentTriangles;
    public Dictionary<Triangle2DPosition, Circle> trianglesChoosenWithCircle;
    public List<Triangle2DPosition> trianglesChoosen;
    public List<Circle> circlesOfTrianglesChoosen;
    [HideInInspector] public List<Segment> polygon;
    [HideInInspector] public List<Triangle2DPosition> trianglesWithoutTrianglesChoosen;
    [HideInInspector] public List<Triangle2DPosition> newTriangles;

    public TriangulationTest()
    {
        pointIsVisible = false;
        currentTriangleAreVisible = false;
        trianglesChoosenAreVisible = false;
        polygonIsVisible = false;
        trianglesWithoutTrianglesChoosenAreVisible = false;
        newTrianglesAreVisible = false;
    }
}