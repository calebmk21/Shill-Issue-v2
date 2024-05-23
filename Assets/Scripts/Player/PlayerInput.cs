using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2f;
    private Rigidbody2D rb; 
    private Vector2 direction = Vector2.zero;
    [SerializeField] private InputActionReference act;
    private bool isNPC = false;
    private float timesInteracted = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(direction.x, direction.y);
    }

    private void OnEnable()
    {
        act.action.performed += Interact;
    }

    private void OnDisable()
    {
        act.action.performed -= Interact;
    }

    void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
        Debug.Log(direction);
    }

    void OnInteract()
    {
        
    }


    void Move(float x, float y)
    {
        rb.velocity = new Vector2(walkSpeed * x, walkSpeed * y);
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isNPC)
        {
            Debug.Log("interacted");
            timesInteracted++;
            if  (timesInteracted > 3) //bool that = true when reach end of dialouge
            {
                Debug.Log("battle start");
                //load battle scene
                timesInteracted = 0; //set bool = false or maybe just set it false when you reload this scene
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            isNPC = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            isNPC = false;
        }
    }


}
