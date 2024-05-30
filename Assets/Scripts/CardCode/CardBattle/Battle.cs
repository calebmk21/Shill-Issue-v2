using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public enum GameplayState { NOT_PLAYING, PLAYING };

    public GameplayState gameplayState = GameplayState.NOT_PLAYING;

    public bool pauseGame = false;

    public float playerHealth = 100;
    public float playerMaxHealth = 100;
    public float playerShield = 30;

    public float currentMana = 0;
    public float maxMana = 100f;

    public int drawNum = 5;
    public float manaGain = 10f;

    //public List<EnemyEntity> enemyUnits = new List<EnemyEntity>();

    public List<ShillIssue.Card> drawPile = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> hand = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> discardPile = new List<ShillIssue.Card>();

    public delegate void OnDiscardCard(ShillIssue.Card card);
    public static event OnDiscardCard onDiscardCard;

    public delegate void OnDrawCard(ShillIssue.Card card);
    public static event OnDrawCard onDrawCard;

    public delegate void OnChangeState(GameplayState newState);
    public static event OnChangeState onChangeState;

    public delegate void OnChangePlayerHealth(float currHealth, float maxHealth);
    public static event OnChangePlayerHealth onChangePlayerHealth;

    public delegate void OnChangeEnemyHealth(float currHealth, float maxHealth);
    public static event OnChangeEnemyHealth onChangeEnemyHealth;

    public delegate void OnChangePlayerMana(float currMana, float maxMana);
    public static event OnChangePlayerMana onChangePlayerMana;

    public delegate void OnChangeEnemyMana(float currMana, float maxMana);
    public static event OnChangeEnemyMana onChangeEnemyMana;

    public delegate void OnPause(bool paused);
    public static event OnPause onPause;

    void Start()
    {
    }

    void Update()
    {    
    }

    public void UpdateResourceUI()
    {
        onChangePlayerHealth?.Invoke(playerHealth, playerMaxHealth);
        //onChangeEnemyHealth?.Invoke(playerHealth, playerMaxHealth);
        onChangePlayerMana?.Invoke(currentMana, maxMana);
        //onChangeEnemyMana?.Invoke(playerHealth, playerMaxHealth);
    }

    public void ChangeMana(float amt)
    {
        currentMana += amt;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        else if (currentMana < 0)
        {
            currentMana = 0;
        }
        UpdateResourceUI();
    }
    
    private void ChangeGameplayState(GameplayState newState)
    {
        gameplayState = newState;
        onChangeState?.Invoke(gameplayState);
    }

    public void StartBattle()
    {
        ChangeGameplayState(GameplayState.PLAYING);
        foreach (ShillIssue.Card card in GameManager._instance.deck)
        {
            drawPile.Add(card);
        }
    }

    public void ShuffleDiscard()
    {
        foreach (ShillIssue.Card card in discardPile)
        {
            drawPile.Add(card);
        }

        drawPile.Shuffle();

        discardPile.Clear();
    }

    public void DiscardCard(int index)
    {
        onDiscardCard?.Invoke(hand[index]);
        discardPile.Add(hand[index]);
        hand.RemoveAt(index);
    }

    public void DrawCards(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (drawPile.Count <= 0)
            {
                ShuffleDiscard();
                if (drawPile.Count == 0)
                {
                    return;
                }
            }
            ShillIssue.Card card = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(card);

            onDrawCard?.Invoke(card);
        }
    }

    public void StartTurn()
    {
        ChangeMana(manaGain);
        DrawCards(drawNum);
    }

    public void EndTurn()
    {
        for (int i = hand.Count - 1; i > -1; i--)
        {
            DiscardCard(i);
        }
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        // Enemy Stuff

        StartTurn();
    }

    public bool IsPlayable(ShillIssue.Card card)
    {
        return card.damageMax <= currentMana;
    }

    public bool PlayCard(ShillIssue.Card card, int index)
    {
        if (!IsPlayable(card)) { return false; }

        foreach (ShillIssue.CardType cardType in card.cardType)
        {
            switch (cardType)
            {
                case ShillIssue.CardType.Dmg:
                    break;
                case ShillIssue.CardType.Heal:
                    GameManager._instance.updatePlayerHealth(card.damageMax); // change to heal value;
                    break;
                default:
                    break;
            }
        }

        DiscardCard(index);
        return true;
    }
}
