using ShillIssue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{

    public List<Card> allCards;

    public GameData()
    {
        this.allCards = new List<Card>();
    }

}
