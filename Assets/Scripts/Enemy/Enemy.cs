using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class Enemy
{
    public string name = "unnamed enemy";
    public EnemyEnum enemyEnum = EnemyEnum.NONE;

    protected bool discardedThisTurn = false;

    public float currentHealth;
    public float maxHealth;

    public float currentMana;
    public float maxMana;
    // How much mana you gain on start of turn
    public float manaGain;

    // Maximum cards you can get on one turn
    public int handSize;

    // List of status that are currently afflicting enemy
    public List<ShillIssue.StatusEffect> statuses = new List<ShillIssue.StatusEffect>();

    public List<ShillIssue.Card> deck = new List<ShillIssue.Card>();

    public List<ShillIssue.Card> drawPile = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> hand = new List<ShillIssue.Card>();
    public List<ShillIssue.Card> discardPile = new List<ShillIssue.Card>();

    protected Battle battle;

    public Enemy(Battle _battle)
    {
        battle = _battle;

        maxHealth = 100f;
        currentHealth = maxHealth;

        maxMana = 100f;
        currentMana = maxMana;

        manaGain = 30f;

        handSize = 5;

        foreach (ShillIssue.Card card in deck)
        {
            drawPile.Add(card);
        }
    }

    public virtual void DoTurn()
    {
        battle.ChangeMana(manaGain, this);
        battle.DrawCards(handSize - hand.Count, this);

        discardedThisTurn = false;

        EnemyAction action;

        while ((action = DetermineAction()).actionType != ActionEnum.EndTurn)
        {
            PlayAction(action);
        }

        battle.DecrementStatuses(this);
    }

    public virtual EnemyAction DetermineAction()
    {
        List<float> scores = new List<float>();

        foreach (ShillIssue.Card card in hand){
            float cardScore = 0;

            if (card.manaCost > currentMana){
                scores.Add(-card.manaCost);
                continue;
            }

            float baseValue = 0;
            float effectRange = 0;

            if (card.cardType.Contains(ShillIssue.CardType.Dmg)){
                baseValue = ((float)(card.damageMin + card.damageMax)) / 2f;
                effectRange = card.damageMax - card.damageMin;

                baseValue *= Mathf.Lerp(1f, 2f, battle.currentHealth / battle.maxHealth);

                cardScore += baseValue;
            }

            if (card.cardType.Contains(ShillIssue.CardType.Heal)){
                baseValue = ((float)(card.healMin + card.healMax)) / 2f;
                effectRange = card.damageMax - card.damageMin;

                baseValue *= Mathf.Lerp(1.8f, 1f, currentHealth / maxHealth);
                baseValue *= Mathf.LerpUnclamped(1f, 1.3f, effectRange / 10f);

                cardScore += baseValue;
            }

            if (card.cardType.Contains(ShillIssue.CardType.Status)){
                for (int i = 0; i < card.statusEffect.Count; i++){
                    baseValue = ((float)card.statusEffect[i].statusNum) * 10f;
                }

                baseValue *= Mathf.Lerp(1f, 2.3f, currentHealth / maxHealth);

                cardScore += baseValue;
            }

            if (card.manaCost == 0){
                cardScore *= 10;
            }
            else{
                cardScore /= (card.manaCost);
            }
            scores.Add(cardScore);
        }

        if (scores.Count == 0){
            return new EnemyAction(ActionEnum.EndTurn);
        }

        float bestScore = scores[0];
        int bestIndex = 0;

        float worstScore = scores[0];
        int worstIndex = 0;

        for (int i = 0; i < scores.Count; i++){
            if (bestScore < scores[i]){
                bestScore = scores[i];
                bestIndex = i;
            }
            if (worstScore > scores[i]){
                worstScore = scores[i];
                worstIndex = i;
            }
        }

        // this will hopefully try not to just play cards to completely 0 mana every turn
        float scoreWall = (currentHealth/maxHealth > 0.5f) ? (maxMana - currentMana) / 3f : 0;

        if (bestScore < scoreWall){
            if (!discardedThisTurn){
                // Discard card
                return new EnemyAction(ActionEnum.DiscardCard, worstIndex);
            }
            return new EnemyAction(ActionEnum.EndTurn);
        }

        return new EnemyAction(ActionEnum.PlayCard, bestIndex);
    }

    public virtual void PlayAction(EnemyAction action)
    {
        switch (action.actionType)
        {
            case ActionEnum.PlayCard:
                battle.PlayCard(hand[action.index], action.index, this);
                break;
            case ActionEnum.DiscardCard:
                battle.DiscardCard(action.index, this);
                discardedThisTurn = true;
                break;
            case ActionEnum.EndTurn: // Should never happen
                break;
            default:
                break;
        }
    }

    // Virtual or not completed?
    public void Die()
    {

    }

    public virtual void AddCards(int numCards = 1){
        if(drawPile.Count == 0){
            Debug.Log("No cards in draw pile");
            return;
        }

        int random = UnityEngine.Random.Range(0, drawPile.Count);
        hand.Add(drawPile[random]);
    }

    // Return index of card with largest attack, -1 if error
    public virtual int GetLargestAttackCard(){
        int index = -1;
        float highestDamageCard = -1f;
        for(int i = 0; i < hand.Count; i++){
            ShillIssue.Card card = hand[i];
            float currentDamage = (card.damageMin + card.damageMax) / 2f;
            if (currentDamage > highestDamageCard){
                if(currentMana >= card.manaCost){
                    highestDamageCard = currentDamage;
                    index = i;
                }
            }
        }

        if(index > -1) { return index; }
        return -1;
    }

    // Return index of card with lowest attack, -1 if error
    public virtual int GetLowestAttackCard(){
        int index = -1;
        float lowestDamageCard = -1f;
        for(int i = 0; i < hand.Count; i++){
            ShillIssue.Card card = hand[i];
            float currentDamage = (card.damageMin + card.damageMax) / 2f;
            if (currentDamage < lowestDamageCard){
                if(currentMana >= card.manaCost){
                    lowestDamageCard = currentDamage;
                    index = i;
                }
            }
        }

        if(index > -1){ return index; }
        return -1;
    }

    // Return index of card with highest mana cost, -1 if erro
    public virtual int GetLargestManaCard(){
        int index = -1;
        int highestManaCard = -1;
        for(int i = 0; i < hand.Count; i++){
            ShillIssue.Card card = hand[i];
            int currentManaCard = card.manaCost;
            if(currentManaCard > highestManaCard){
                if(currentMana >= card.manaCost){
                    highestManaCard = currentManaCard;
                    index = i;
                }
            }
        }

        if(index > -1){ return index; }
        return -1;
    }

    // Return index of card with lowest mana cost, -1 if error
    public virtual int GetLowestManaCard(){
        int index = -1;
        int lowestManaCard = -1;
        for(int i = 0; i < hand.Count; i++){
            ShillIssue.Card card = hand[i];
            int currentManaCard = card.manaCost;
            if(currentManaCard < lowestManaCard){
                if(currentMana >= card.manaCost){
                    lowestManaCard = currentManaCard;
                    index = i; 
                }
            }
        }

        if(index > -1){ return index; }
        return -1;
    }

    // Return index of card with highest health, -1 if error
    public virtual int GetHighestHealCard(){
        int index = -1;
        float highestHealCard = -1;
        for(int i = 0; i < hand.Count; i++){
            ShillIssue.Card card = hand[i];
            if(card.cardType.Contains(ShillIssue.CardType.Heal)){
                float currentHealing = (float)(card.healMax + card.healMin) / 2f;
                if(currentHealing > highestHealCard){
                    if(currentMana >= card.manaCost){
                        highestHealCard = currentHealing;
                        index = i;
                    }
                }
            }
        }

        if(index > -1){ return index; }
        return -1;
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