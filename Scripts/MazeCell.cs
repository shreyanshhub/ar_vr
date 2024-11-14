using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public bool isWall = false;
    public Vector3 worldPosition;
    public int gridX;
    public int gridZ;
    public float fCost;
    public float gCost;
    public float hCost;
    public MazeCell parent;
    
    public void Initialize(bool isWall, Vector3 position, int x, int z)
    {
        this.isWall = isWall;
        this.worldPosition = position;
        this.gridX = x;
        this.gridZ = z;
    }
    
    public float FCost
    {
        get { return gCost + hCost; }
    }
}
