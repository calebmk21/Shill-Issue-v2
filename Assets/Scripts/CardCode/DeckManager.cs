using System;
using System.Collections;
using System.Collections.Generic;
using ShillIssue;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class DeckManager : MonoBehaviour, IDataPersistence
{
    public SerializableList<Card> allCards = new SerializableList<Card>();
    public int startingHandSize;

    public HandManager handManager;

    private int currentIndex = 0;
    public int maxHandSize;
    // Currently 2, but you can change it however you want
    public int currentHandSize;

    void Start()
    {
        //Load all card assets from the Resources folder
        Card[] cards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);

        //handManager = FindObjectOfType<HandManager>();
        //handManager = GetComponent<HandManager>();

        // Set values derp
        //maxHandSize = handManager.maxHandSize;
        startingHandSize = 4;
        //currentHandSize = startingHandSize;
        //if(startingHandSize > maxHandSize){
        //    Debug.Log("Starting hand size is too large");
        //}
        //for (int i = 0; i < startingHandSize; i++)
        //{
        //    Debug.Log($"Drawing Card");
        //    DrawCard(handManager);
        //}
    }

    void OnEnable()
    {
        Battle.onDrawCard += DrawCard;
    }

    void OnDisable()
    {
        Battle.onDrawCard -= DrawCard;
    }


    void Update()
    {
        //    if (handManager != null)
        //    {
        //        currentHandSize = handManager.cardsInHand.Count;
        //    }
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0)
            return;

        if (currentHandSize < maxHandSize)
        {
            Debug.Log("Drawing card");
            // Get random card
            currentIndex = UnityEngine.Random.Range(0, allCards.Count);
            Card nextCard = allCards[currentIndex];

            // Add card
            handManager.AddCardToHand(nextCard);
            currentHandSize++;
        }
    }

    public void DrawCard(Card card)
    {
        handManager.AddCardToHand(card);
    }

    public void LoadData(GameData data)
    {
        this.allCards = data.allCards;
    }

    public void SaveData(ref GameData data)
    {
        data.allCards = this.allCards;
    }
}
