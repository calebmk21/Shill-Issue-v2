using System.Collections;
using System.Collections.Generic;
using ShillIssue;
using UnityEngine;

public class WinCard : MonoBehaviour
{
    public List<Card> systemCards = new List<Card>();
    // NOTE: This should be shared during the whole game
    // but for now it's just a new list
    public List<Card> currentCards = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        Card[] allCards = Resources.LoadAll<Card>("Cards");
        systemCards.AddRange(allCards);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Add Win Card")]
    public void AddWinCard(){
        // Edge case
        if(systemCards.Count == 0){ return; }

        if(currentCards.Count < systemCards.Count){
            int currentIndex = UnityEngine.Random.Range(0, systemCards.Count);
            // Cycle until you find a unique card
            for(int i = 0; i < systemCards.Count; i++){
                if(currentCards.Contains(systemCards[currentIndex])){
                    currentIndex = (currentIndex + 1) % systemCards.Count;
                } else {
                    currentCards.Add(systemCards[currentIndex]);
                    break;
                }
            }
        } else {
            Debug.Log("User has already collected all win cards");
        }
    }
}
