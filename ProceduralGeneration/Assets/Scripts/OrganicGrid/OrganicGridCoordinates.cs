using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrganicGrid
{
public class OrganicGridCoordinates
{
    public Rect GridRect;
    public Vector3 StartPosition;

    public OrganicGridCoordinates(Rect _gridRect, Vector3 _startPosition)
    {
        GridRect = _gridRect;
        StartPosition = _startPosition;
    }
}

}

