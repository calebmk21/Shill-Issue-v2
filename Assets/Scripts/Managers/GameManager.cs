using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public enum CardEnum
    {
        Card1, Card2
    }

    [SerializeField]
    private List<CardEnumMatch> cardDictSetup = new List<CardEnumMatch>();
    public Dictionary<CardEnum, ShillIssue.Card> entityObjectDict = new Dictionary<CardEnum, ShillIssue.Card>();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }

        foreach (CardEnumMatch match in cardDictSetup)
        {
            entityObjectDict.Add(match.cardType, match.cardData);
        }
    }
}
