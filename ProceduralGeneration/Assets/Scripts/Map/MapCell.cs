using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapCell
{
 public int Id;
 public MapFragment Fragment;
 MapCell(int id, MapFragment fragment )
 {
  Id = id;
  Fragment = fragment; 
 }
}
