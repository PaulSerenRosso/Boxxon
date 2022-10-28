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
 public LinearEquation(float _a, float _b, float _x, float _y)
 {
  a = _a;
  b = _b; 
  x = _x;
  y = _y;

 }
}
}
