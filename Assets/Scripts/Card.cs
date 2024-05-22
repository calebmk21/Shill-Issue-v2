using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameManager gm;
    public bool hasBeenPlayed;
    public int handIndex;

    private void Start(){
        gm = FindObjectOfType<GameManager>();   
    }

    private void OnMouseDown(){
        if(hasBeenPlayed == false){
            transform.position += Vector3.up *2;
            hasBeenPlayed = true;
            gm.availableCardSlots[handIndex] = true;  
            Invoke("MoveToDiscardPile", 2f); 
        }
    }

    void MoveToDiscardPile(){
        gm.discardPile.Add(this);
        gameObject.SetActive(false);    
    }
    
}
