using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelper 
{
    public static bool InputMouseIsInCanvasArea(this RectTransform _canvasArea)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(_canvasArea, Input.mousePosition))
        {
            return true;
        }
        return false;
    }
    public static Vector3 GetWorldPositionOfCanvasElement(this RectTransform _element) // Find world point of canvas elements
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_element, _element.position, Camera.main, out var result);
        return result;
    }
    public static Vector2 GetCanvasPositionOfWorldElement(this Transform _element) // Find world point of canvas elements
    {
        return RectTransformUtility.WorldToScreenPoint(Camera.main, _element.position);;
    }
}
