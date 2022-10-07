using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ListArrayHelper
{
    public static void ShuffleArray<T>(this Random _rng, T[] _array)
    {
        int n = _array.Length;
        while (n > 1) 
        {
            int k = _rng.Next(n--);
            (_array[n], _array[k]) = (_array[k], _array[n]);
        }
    }
    public static void ShuffleList<T>(this Random _rng, List<T> _list)
    {
        int n = _list.Count;
        while (n > 1) 
        {
            int k = _rng.Next(n--);
            (_list[n], _list[k]) = (_list[k], _list[n]);
        }
    }
}
