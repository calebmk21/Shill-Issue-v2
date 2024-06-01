using ShillIssue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{

    public SerializableList<Card> allCards;

    public Vector3 playerPos;

    public GameData()
    {
        this.allCards = new SerializableList<Card>();
        playerPos = Vector3.zero;
    }

}
