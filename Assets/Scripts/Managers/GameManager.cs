using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
 {
    public static GameManager _instance { get; private set;}
    public OptionsManager OptionsManager { get; private set; }
    public AudioManager AudioManager{ get; private set; }
    public DeckManager DeckManager{ get; private set; }

    private int playerHealth;
    private int playerMaxHealth;
    private int difficulty = 5;

    [SerializeField]
    private List<CardEnumMatch> cardDictSetup = new List<CardEnumMatch>();
    public Dictionary<CardEnum, ShillIssue.Card> entityObjectDict = new Dictionary<CardEnum, ShillIssue.Card>();

    public List<ShillIssue.Card> deck = new List<ShillIssue.Card>();


    public enum CardEnum
    {
        Card1, Card2
    }

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
            InitializeManagers();
        }

        foreach (CardEnumMatch match in cardDictSetup)
        {
            entityObjectDict.Add(match.cardType, match.cardData);
        }
    }

    public static class GameData
    {
        public static Vector3 playerPosition;
    }


    private void InitializeManagers()
    {
        OptionsManager = GetComponentInChildren<OptionsManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        DeckManager = GetComponentInChildren<DeckManager>();

        if( OptionsManager == null){
            GameObject perfab = Resources.Load<GameObject>("Perbabs/OptionsManager");
            if (perfab == null){
                Debug.Log($"OptionsManager perfab not found");
            } else {
                Instantiate(perfab, transform.position, Quaternion.identity, transform);
                OptionsManager = GetComponentInChildren<OptionsManager>();
            }
        }
        if( AudioManager == null){
            GameObject perfab = Resources.Load<GameObject>("Perbabs/AudioManager");
            if (perfab == null){
                Debug.Log($"AudioManager perfab not found");
            } else {
                Instantiate(perfab, transform.position, Quaternion.identity, transform);
                AudioManager = GetComponentInChildren<AudioManager>();
            }
        }
        if( DeckManager == null){
            GameObject perfab = Resources.Load<GameObject>("Perbabs/DeckManager");
            if (perfab == null){
                Debug.Log($"DeckManager perfab not found");
            } else {
                Instantiate(perfab, transform.position, Quaternion.identity, transform);
                DeckManager = GetComponentInChildren<DeckManager>();
            }
        }

    }

    public int PlayerHealth
    {
        get{ return playerHealth; }
        set {playerHealth = value; }
    }
    public int PlayerMaxHealth
    {
        get { return playerMaxHealth; }
        set { playerMaxHealth = value; }
    }

    public void updatePlayerHealth(int val)
    {
        playerHealth += val;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }

        if (playerHealth < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        ;
    }

    public int Difficulty
    {
        get{ return difficulty; }
        set { difficulty = value; }
    }
}
