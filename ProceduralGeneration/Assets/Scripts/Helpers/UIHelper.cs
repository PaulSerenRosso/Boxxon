using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelper 
{
    public static bool InputMouseIsInCanvasArea(RectTransform _canvasArea)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(_canvasArea, Input.mousePosition))
        {
            return true;
        }
        return false;
    }
}
