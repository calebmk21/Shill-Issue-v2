using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    public string name = "unnamed enemy";
    public EnemyEnum enemyEnum = EnemyEnum.NONE;

    public float currentHealth;
    public float maxHealth;

    public float currentMana;
    public float maxMana;
    public float manaGain;

    public int handSize;

    public List<ShillIssue.StatusEffect> statuses = new List<ShillIssue.StatusEffect>();

    public List<ShillIssue.Card> deck = new List<ShillIssue.Card>();

    public List<ShillIssue.Card> drawPile = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> hand = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> discardPile = new List<ShillIssue.Card>();

    private Battle battle;

    public Enemy(Battle _battle)
    {
        battle = _battle;

        foreach (ShillIssue.Card card in deck)
        {
            drawPile.Add(card);
        }
    }

    public virtual void DoTurn()
    {
        battle.ChangeMana(manaGain, this);
        battle.DrawCards(handSize - hand.Count, this);

        EnemyAction action;

        while ((action = DetermineAction()).actionType != ActionEnum.EndTurn)
        {
            PlayAction(action);
        }

        battle.DecrementStatuses(this);
    }

    public virtual EnemyAction DetermineAction()
    {
        return new EnemyAction(ActionEnum.NONE);
    }

    public virtual void PlayAction(EnemyAction action)
    {
        switch (action.actionType)
        {
            case ActionEnum.PlayCard:
                battle.PlayCard(hand[action.index], action.index, this);
                break;
            case ActionEnum.DiscardCard:
                break;
            case ActionEnum.EndTurn:
                break;
            default:
                break;
        }
    }

    public void Die()
    {

    }
}

public class EnemyAction
{
    public ActionEnum actionType;
    public int index;

    public EnemyAction(ActionEnum _actionType, int _index = -1)
    {
        actionType = _actionType;
        index = _index;
    }
}

public enum EnemyEnum
{
    NONE, 
    ExampleEnemy
}

public enum ActionEnum
{
    NONE,
    PlayCard,
    DiscardCard,
    EndTurn
}