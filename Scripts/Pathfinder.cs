using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{
    private MazeGenerator mazeGenerator;
    private List<MazeCell> path;
    public Material pathMaterial; // Material to show the path
    private List<GameObject> pathVisuals = new List<GameObject>();
    
    void Start()
    {
        mazeGenerator = FindFirstObjectByType<MazeGenerator>();
    }
    
    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        ClearPath(); // Clear old path
        
        MazeCell startCell = GetClosestCell(startPos);
        MazeCell targetCell = GetClosestCell(targetPos);
        
        if (startCell == null || targetCell == null || startCell.isWall || targetCell.isWall)
            return;
            
        List<MazeCell> openSet = new List<MazeCell>();
        HashSet<MazeCell> closedSet = new HashSet<MazeCell>();
        openSet.Add(startCell);
        
        while (openSet.Count > 0)
        {
            MazeCell currentCell = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentCell.FCost || 
                    openSet[i].FCost == currentCell.FCost && openSet[i].hCost < currentCell.hCost)
                {
                    currentCell = openSet[i];
                }
            }
            
            openSet.Remove(currentCell);
            closedSet.Add(currentCell);
            
            if (currentCell == targetCell)
            {
                RetracePath(startCell, targetCell);
                return;
            }
            
            foreach (MazeCell neighbor in mazeGenerator.GetNeighbors(currentCell))
            {
                if (neighbor.isWall || closedSet.Contains(neighbor))
                    continue;
                    
                float newMovementCostToNeighbor = currentCell.gCost + GetDistance(currentCell, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetCell);
                    neighbor.parent = currentCell;
                    
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
    }
    
    private MazeCell GetClosestCell(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / mazeGenerator.cellSize);
        int z = Mathf.RoundToInt(worldPosition.z / mazeGenerator.cellSize);
        
        if (x >= 0 && x < mazeGenerator.mazeWidth && z >= 0 && z < mazeGenerator.mazeLength)
        {
            return mazeGenerator.grid[x, z];
        }
        return null;
    }
    
    private float GetDistance(MazeCell cellA, MazeCell cellB)
    {
        int distX = Mathf.Abs(cellA.gridX - cellB.gridX);
        int distZ = Mathf.Abs(cellA.gridZ - cellB.gridZ);
        
        if (distX > distZ)
            return 14 * distZ + 10 * (distX - distZ);
        return 14 * distX + 10 * (distZ - distX);
    }
    
    private void RetracePath(MazeCell startCell, MazeCell endCell)
    {
        path = new List<MazeCell>();
        MazeCell currentCell = endCell;
        
        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.parent;
        }
        path.Add(startCell);
        path.Reverse();
        
        VisualizePath();
    }
    
    private void VisualizePath()
    {
        foreach (MazeCell cell in path)
        {
            GameObject pathMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pathMarker.transform.position = cell.worldPosition + Vector3.up * 0.5f;
            pathMarker.transform.localScale = Vector3.one * 0.3f;
            
            if (pathMaterial != null)
                pathMarker.GetComponent<Renderer>().material = pathMaterial;
                
            pathVisuals.Add(pathMarker);
        }
    }
    
    private void ClearPath()
    {
        foreach (GameObject obj in pathVisuals)
        {
            Destroy(obj);
        }
        pathVisuals.Clear();
    }
}