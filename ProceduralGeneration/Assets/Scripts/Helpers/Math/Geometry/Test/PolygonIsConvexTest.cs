using System;
using System.Collections;
using System.Collections.Generic;
using GeometryHelpers;
using UnityEngine;

public class PolygonIsConvexTest : MonoBehaviour
{
    [SerializeField] private Vector2[] vertices;

    [SerializeField] private bool isConvex = true;

    private void OnValidate()
    {
        if (vertices.Length > 3)
        {
            if (GeometryHelper.CheckIfPolygonIsConvex(vertices))
            {
                isConvex = true;
            }
            else
            {
                isConvex = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (vertices.Length > 3)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(vertices[0], vertices[vertices.Length - 1]);
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Gizmos.DrawLine(vertices[i], vertices[i + 1]);
            }
        }
    }
}