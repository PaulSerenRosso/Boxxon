using System;
using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;

public class OrganicGrid : MonoBehaviour
{
    [SerializeField]
    private PointObjectGenerator pointObjectGenerator;

    [SerializeField] private ObjectTriangulation objectTriangulation;

    [SerializeField] private Rect organicGridRect;
    private void Start()
    {
        pointObjectGenerator.LaunchPointObjectGenerator();
        objectTriangulation.LaunchObjectTriangulation();
    }
}
