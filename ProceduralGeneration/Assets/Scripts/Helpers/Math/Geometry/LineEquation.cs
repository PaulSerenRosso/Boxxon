using System;

namespace GeometryHelpers
{
 [Serializable]
 public struct LinearEquation
{
 public float a;
 public float b;
 public float x;
 public float y;
 public bool hasX;
  public bool hasY ;
 public LinearEquation(float _a, float _b, float _x, float _y, bool _hasX, bool _hasY)
 {
  a = _a;
  b = _b; 
  x = _x;
  y = _y;
 hasX = _hasX;
 hasY = _hasY;
 }
}
}
