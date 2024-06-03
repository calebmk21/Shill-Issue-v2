using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IDataPersistence
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.zero;
    [SerializeField] private InputActionReference act;
    private bool isNPC = false;
    private EnemyEnum enemyType;
    private int timesInteracted = 0;
    private SceneSwitcher sceneSwitcher;
    private VisualNovel visualNovel; 
    private string[] dialogueNode = {"Battle_1", "Battle_2", "Battle_3", "Battle_4"}; 
    private bool canBattle = false; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        visualNovel = FindObjectOfType<VisualNovel>();

        // Load player position if returning to the overworld scene
        if (sceneSwitcher != null && sceneSwitcher.IsOverworldScene())
        {
            transform.position = GameManager.GameData.playerPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move(direction.x, direction.y);

        // Check for the "C" key press to change scenes
        if (Keyboard.current.cKey.wasPressedThisFrame && sceneSwitcher != null)
        {
            sceneSwitcher.SwitchScene();
        }
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
        //Debug.Log(direction);
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
            if (visualNovel != null && dialogueNode.Length > timesInteracted) {
                if (canBattle && sceneSwitcher != null)
                {
                    Debug.Log("battle start");
                    sceneSwitcher.StartBattle(enemyType);
                    canBattle = false; 
                }
                else {
                    // change this to be programmatic based on NPC
                    visualNovel.StartDialogue(dialogueNode[timesInteracted]);
                    timesInteracted++;
                    canBattle = true; 
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            isNPC = true;
            enemyType = other.GetComponent<NPC>().enemyType;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            isNPC = false;
        }
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPos;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPos = this.transform.position;
    }

}
