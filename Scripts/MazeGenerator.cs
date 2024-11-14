using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public int mazeWidth = 10;
    public int mazeLength = 10;
    public float cellSize = 2f;
    
    [HideInInspector]
    public MazeCell[,] grid;
    
    void Start()
    {
        GenerateMaze();
    }
    
    void GenerateMaze()
    {
        grid = new MazeCell[mazeWidth, mazeLength];
        
        // Create the basic grid
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeLength; z++)
            {
                Vector3 worldPos = new Vector3(x * cellSize, 0, z * cellSize);
                
                // Create walls on borders and randomly inside
                bool isWall = (x == 0 || x == mazeWidth - 1 || z == 0 || z == mazeLength - 1 ||
                             Random.Range(0f, 1f) < 0.3f);
                
                GameObject prefabToUse = isWall ? wallPrefab : floorPrefab;
                GameObject cell = Instantiate(prefabToUse, worldPos, Quaternion.identity);
                cell.transform.parent = transform;
                
                MazeCell mazeCell = cell.AddComponent<MazeCell>();
                mazeCell.Initialize(isWall, worldPos, x, z);
                
                grid[x, z] = mazeCell;
            }
        }
    }
    
    public List<MazeCell> GetNeighbors(MazeCell cell)
    {
        List<MazeCell> neighbors = new List<MazeCell>();
        
        // Check all adjacent cells
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                
                int checkX = cell.gridX + x;
                int checkZ = cell.gridZ + z;
                
                if (checkX >= 0 && checkX < mazeWidth && checkZ >= 0 && checkZ < mazeLength)
                {
                    neighbors.Add(grid[checkX, checkZ]);
                }
            }
        }
        
        return neighbors;
    }
}
