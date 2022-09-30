using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PointGenerator
{
    public class PoissonDiskSampling 
    {
        public Vector2 GridSize
        {
            set { gridSize = value; }
        }

        public float MinDistanceBetweenPoints
        {
            set { minDistanceBetweenPoints = value; }
        }

        public float MaxDistanceBetweenPoints
        {
            set { maxDistanceBetweenPoints = value; }
        }

        private Vector2 gridSize;
        private float minDistanceBetweenPoints;
        private float maxDistanceBetweenPoints;
        private int[,] grid;
        private float cellSize;
        private List<Vector2> spawnPoints;
        private List<Vector2> points;
        

        public PoissonDiskSampling(Vector2 _gridSize, float _minDistanceBetweenPoints, float _maxDistanceBetweenPoints)
        {
            gridSize = _gridSize;
            minDistanceBetweenPoints = _minDistanceBetweenPoints;
            maxDistanceBetweenPoints = _maxDistanceBetweenPoints;
            if (gridSize.x <= 0 || gridSize.y <= 0 || minDistanceBetweenPoints <= 0 || maxDistanceBetweenPoints <= 0)
                throw new Exception("Points Generator arguments is not valid");
        }
        
        public List<Vector2> GeneratePoints(
            int iterationForFindNewPoint = 30)
        {
            Initialize();
            IterateGeneration(iterationForFindNewPoint);
            return points;
        }

        void Initialize()
        {
            cellSize = minDistanceBetweenPoints / Mathf.Sqrt(2);
            grid = new int[Mathf.CeilToInt(gridSize.x / cellSize), Mathf.CeilToInt(gridSize.y / cellSize)];
            points = new List<Vector2>();
            spawnPoints = new List<Vector2>();
        }

        void IterateGeneration(int iterationForFindNewPoint)
        {
            spawnPoints.Add(gridSize / 2);

            while (spawnPoints.Count > 0)
            {
            bool isValided = false;
                    Vector2 randomSpawnPoint = SelectRandomPoint(spawnPoints);
                for (int i = 0; i < iterationForFindNewPoint; i++)
                {
                    Vector2 newPoint = CreateRandomPoint(randomSpawnPoint);
                    if (CheckPointIsValided(newPoint))
                    {
                        isValided = true;
                        AddPoint(newPoint);
                        break;
                    }
                }
                if (!isValided)
                {
                    spawnPoints.Remove(randomSpawnPoint);
                }
                
            }
        }
        Vector2 SelectRandomPoint(List<Vector2> _points)
        {
            int randomSpawnPointIndex = Random.Range(0, _points.Count);
            return _points[randomSpawnPointIndex];
        }
        Vector2 CreateRandomPoint(Vector2 _randomSpawnPoint)
        {
            Vector2 randomPoint = _randomSpawnPoint + MathHelper.RandomDirection()
                * Random.Range(minDistanceBetweenPoints, maxDistanceBetweenPoints);
            return randomPoint;
        }

        void AddPoint(Vector2 _point)
        {
            points.Add(_point);
            spawnPoints.Add((_point));
            grid[(int)(_point.x / cellSize), (int)(_point.y / cellSize)] = points.Count;
        }

        bool CheckPointIsValided(Vector2 _point)
        {
            if (PointIsInGrid(_point))
            {
                int2 pointCellIndex = GetPointCellIndex(_point);
                int2 minNeighboursCellsIndex = GetMinNeighboursCellsIndex(pointCellIndex);
                int2 maxNeighboursCellsIndex = GetMaxNeighboursCellsIndex(pointCellIndex);
                List<Vector2> neighbours = new List<Vector2>();
                for (int x = minNeighboursCellsIndex.x; x <= maxNeighboursCellsIndex.x; x++)
                {
                    for (int y = minNeighboursCellsIndex.y; y <= maxNeighboursCellsIndex.y; y++)
                    {
                        int currentCell = grid[x,y]-1;
                        if (currentCell!= -1)
                        {
                            if (_point.InMinDistance(points[currentCell], minDistanceBetweenPoints))
                            {
                                return false;
                            }
                            neighbours.Add(points[currentCell]);
                        }
                    }
                }

                for (int i = 0; i < neighbours.Count; i++)
                {
                Debug.Log(_point +"   "+pointCellIndex + "    "+ minNeighboursCellsIndex+"   "+maxNeighboursCellsIndex+ "   " + neighbours[i]);
                
                }
                return true;
            }
            return false;
        }


        bool PointIsInGrid(Vector3 point)
        {
            if (point.x.IsClamp(0, gridSize.x) && point.y.IsClamp(0, gridSize.y))
            {
                return true;
            }

            return false;
        }
        int2 GetPointCellIndex(Vector2 point)
        {
            return new int2((int)(point.x / cellSize), (int)(point.y / cellSize));
          
        }
        int2 GetMinNeighboursCellsIndex(int2 pointCellIndex)
        {
            int2 minPoint = new int2(Mathf.Max(0, pointCellIndex.x - 2), Mathf.Max(0, pointCellIndex.y - 2));
            return  minPoint;
        }

        int2 GetMaxNeighboursCellsIndex(int2 pointCellIndex)
        {
            int2 maxPoint = new int2(Mathf.Min(pointCellIndex.x + 2, grid.GetLength(0) - 1),
                Mathf.Min(pointCellIndex.y + 2, grid.GetLength(1) - 1));
            return maxPoint;
        }

        public Vector2[] GeneratePoints()
        {
            throw new NotImplementedException();
        }
    }
}
