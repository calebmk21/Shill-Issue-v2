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

    //public List<EnemyEntity> enemyUnits = new List<EnemyEntity>();

    public List<ShillIssue.Card> drawPile = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> hand = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> discardPile = new List<ShillIssue.Card>();

    public delegate void OnDiscardSpell(ShillIssue.Card spell);
    public static event OnDiscardSpell onDiscardSpell;

    public delegate void OnDrawSpell(ShillIssue.Card spell);
    public static event OnDrawSpell onDrawSpell;

    public delegate void OnChangeState(GameplayState newState);
    public static event OnChangeState onChangeState;

    public delegate void OnChangeResource();
    public static event OnChangeResource onChangeResource;

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
        onChangeResource?.Invoke();
    }
    
    private void ChangeGameplayState(GameplayState newState)
    {
        gameplayState = newState;
        onChangeState?.Invoke(gameplayState);
    }

    public void StartBattle()
    {
        ChangeGameplayState(GameplayState.PLAYING);
        foreach (ShillIssue.Card spell in GameManager._instance.deck)
        {
            drawPile.Add(spell);
        }
    }

    public void ShuffleDiscard()
    {
        foreach (ShillIssue.Card spell in discardPile)
        {
            drawPile.Add(spell);
        }

        drawPile.Shuffle();

        discardPile.Clear();
    }

    public void DiscardSpell(ShillIssue.Card spell)
    {
        discardPile.Add(spell);
        hand.Remove(spell);
        onDiscardSpell?.Invoke(spell);
    }

    public void DrawSpells(int num)
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
            ShillIssue.Card spell = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(spell);

            onDrawSpell?.Invoke(spell);
        }
    }

    public void QueueNextHand()
    {
        foreach (ShillIssue.Card spell in hand)
        {
            discardPile.Add(spell);
        }

        hand.Clear();

        DrawSpells(drawNum);
    }

    public void EndTurn()
    {
        for (int i = hand.Count - 1; i > -1; i--)
        {
            DiscardSpell(hand[i]);
        }
    }

    public void PlayCard(ShillIssue.Card card)
    {
        foreach (ShillIssue.CardType cardType in card.cardType)
        {
            switch (cardType)
            {
                case ShillIssue.CardType.Dmg:
                    break;
                case ShillIssue.CardType.Heal:
                    break;
                default:
                    break;
            }
        }
    }
}
