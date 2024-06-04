using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudio : MonoBehaviour
{
    [SerializeField] private AudioSource continueSound;
    [SerializeField] private AudioSource typingSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void continueButton()
    {
        continueSound.Play();
    }

    public void typewritter()
    {
        typingSound.Play();
    }
}
