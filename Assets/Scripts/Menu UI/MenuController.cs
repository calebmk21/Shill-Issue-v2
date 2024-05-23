using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    
    [Header("Audio")]
    public GameObject placeholder;
    
    
    [Header("Panels")]
    public GameObject pauseMenu;
    public GameObject currentPanel;
    public GameObject newPanel;
    

    

    public static bool isPaused = false;
    
    // Note to self: un-comment the audio stuff once an audio listener is made
    
    
    private void Awake()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // This is mostly for using Esc to pause in game, make sure to attach this script to the player as well
    public void OnPause()
    {
        Pause();
    }

    public void Pause()
    {
        if (isPaused == true)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            //audioManager.PlayAudio(audioManager.background);
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            //audioManager.PauseAudio(audioManager.background);
        }
    }
    
    // Basic scene loader
    public void NavButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    // It quits the game lol
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit the application.");
    }

    // Simple panel swap method -- good for basic navigation, but can spaghetti fast! Use sparingly
    public void PanelSwap()
    {
        currentPanel.SetActive(false);
        newPanel.SetActive(true);
    }
    

}