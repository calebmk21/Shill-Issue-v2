using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2f;
    private Rigidbody2D rb; 
    private Vector2 direction = Vector2.zero;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(direction.x, direction.y);
    }

    void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
        Debug.Log(direction);
    }

    void OnInteract()
    {
        Debug.Log("Interacted");
    }


    void Move(float x, float y)
    {
        rb.velocity = new Vector2(walkSpeed * x, walkSpeed * y);
    }

    void Interact()
    {
        
    }
    
    
    
}
