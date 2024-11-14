using UnityEngine;

public class PathTester : MonoBehaviour
{
    private Pathfinder pathfinder;
    
    void Start()
    {
        pathfinder = FindFirstObjectByType<Pathfinder>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                pathfinder.FindPath(transform.position, hit.point);
            }
        }
    }
}
