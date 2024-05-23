using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShillIssue;
using System;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    // Assign card prefab in inspector
    public GameObject cardPrefab; 
    //Root of the hand position
    public Transform handTransform; 
    public float fanSpread = 7f;
    public float cardSpacing = -100f;
    public float verticalSpacing = 70f;

    //Hold a list of the card objects in the hand
    public List<GameObject> cardsInHand = new List<GameObject>();

    void Start()
    {
    }

    public void AddCardToHand(Card cardData)
    {
        //Insstantiate teh card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        //Set the CardData of the instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpadateHandVisuals();
    }

    void Update(){
        UpadateHandVisuals();
    }

    private void UpadateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1){
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f,0f);
            return;
        }


        for(int i = 0; i < cardCount; i++){
            float rotationAngel = (fanSpread * (i - (cardCount -1)/2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngel);

            float horizontalOffset = (cardSpacing * (i - (cardCount -1)/2f));
            //Normalize card postion between -1 and 1
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalSpacing * (1- normalizedPosition * normalizedPosition);
            
            // set card positions
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset,0f);
        }
    }
}
