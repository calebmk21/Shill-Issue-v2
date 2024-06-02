using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();

        // Load player position if returning to the overworld scene
        if (IsOverworldScene() && GameManager.GameData.playerPosition != Vector3.zero)
        {
            if (playerInput != null)
            {
                playerInput.transform.position = GameManager.GameData.playerPosition;
            }
        }
    }

    void Update()
    {
        // Check for the "C" key press to change scenes using the new Input System
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            SwitchScene();
        }
    }

    public void SwitchScene()
    {
        if (IsOverworldScene())
        {
            // Save player position before switching to the card battle scene
            if (playerInput != null)
            {
                GameManager.GameData.playerPosition = playerInput.transform.position;
            }
            SceneManager.LoadScene("CardBattleCombined");
        }
        else if (SceneManager.GetActiveScene().name == "CardBattleCombined")
        {
            // Load back to the overworld scene
            SceneManager.LoadScene("OverworldCombined");
        }
    }

    public void StartBattle()
    {
        // Save player position before switching to the card battle scene
        if (playerInput != null)
        {
            GameManager.GameData.playerPosition = playerInput.transform.position;
        }
        GameManager._instance.currentEnemy = EnemyEnum.ExampleEnemy;
        SceneManager.LoadScene("CardBattleCombined");
    }

    public bool IsOverworldScene()
    {
        return SceneManager.GetActiveScene().name == "OverworldCombined";
    }
}
