using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private CharacterController controller;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Calculate movement direction
        Vector3 movement = transform.forward * vertical;
        
        // Apply movement
        controller.Move(movement * moveSpeed * Time.deltaTime);
        
        // Handle rotation
        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
        
        // Apply gravity
        controller.Move(Physics.gravity * Time.deltaTime);
    }
}