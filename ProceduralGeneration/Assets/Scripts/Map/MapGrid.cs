using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    private MapCell[] _mapCellList;
    [SerializeField]
    private int3 _cellLineCount;
    [SerializeField]
    private float3 _cellSize;

    void Create()
    {
        int _cellCount = _cellLineCount.x * _cellLineCount.y * _cellLineCount.z;
        _mapCellList = new MapCell[_cellCount];
        for (int i = 0; i < _cellLineCount.x; i++)
        {
            for (int j = 0; j < _cellLineCount.y; j++)
            {
                for (int z = 0; z < _cellLineCount.z; z++)
                {
                    
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
