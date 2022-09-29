using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointGeneratable 
{
    public Vector2[] GeneratePoints()
    {
        return new Vector2[0];
    }
}
