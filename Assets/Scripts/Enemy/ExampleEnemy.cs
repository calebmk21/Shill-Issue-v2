using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Callbacks;
using UnityEngine;

public class ExampleEnemy : Enemy {

    // Cards left this turn
    public int cardsLeft = 5;
    // From 0-3
    public int currentTurn = 0;
    public int numTurns = 4;

    public ExampleEnemy(Battle _battle) : base(_battle) {
        name = "Example enemy";
        enemyEnum = EnemyEnum.ExampleEnemy;
        cardsLeft = 5;
        currentTurn = 0;

        drawPile = new List<ShillIssue.Card>()
        {
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2],
            GameManager._instance.entityObjectDict[GameManager.CardEnum.Card2]
        };

        // Add cards to hand
        //for(int i = 0; i < handSize; i++){
        //    AddCards();
        //}
    }

    // Add mana, cards
    public void OnStartTurn() {
       battle.ChangeMana(manaGain);
       cardsLeft = 5;
       for(int i = 0; i < handSize; i++){
            AddCards();
       }
    }

    // Not really needed
    public EnemyAction OnEndTurn(){
        return new EnemyAction(ActionEnum.EndTurn);
    }

    // Runs at start of turn, before any greedy algorithms
    // "Brute" enemy
    // -1 implies this card doesn't exist
    public int PreCheck(){
        // Get highest damage card
        int index = GetLargestAttackCard();
        ShillIssue.Card card;
        if(index != -1){
            card = hand[index];
        } else {
            return -1;
        }

        float attackAmount = (card.damageMax + card.damageMin) / 2f;

        // Check if card >= 50% of player's health, -1 if doesn't exist
        if((battle.currentHealth * 0.5f) <= attackAmount){
            return index;
        }

        return -1;
    }

    // Return -1 if no card found
    public int Turn1() {
        // Choose card with most damage either in one time or multiple goes
        int index = GetLargestAttackCard();
        if(index != -1){
            return index;
        }
        return -1;
    }

    // Return -1 if no card found
    public int Turn2(){
        // Choose card with least mana
        int index = GetLowestManaCard();
        if(index != -1){
            return index;
        }

        return -1;
    }

    // Return -1 if no card found
    public int Turn3(){
        // Choose card with most mana
        int index = GetLargestAttackCard();
        if(index != -1){
            return index;
        }

        return -1;
    }

    public int Turn4(){
        // Remove card with least health
        int index = GetLowestHealCard();
        if(index != -1){
            return index;
        }

        return -1;
    }

    public override EnemyAction DetermineAction(){

        return base.DetermineAction();

        // Calculate turn
        currentTurn = currentTurn % numTurns;
        // Edge cases
        if((cardsLeft <= 0) && (hand.Count != 0)){
            return new EnemyAction(ActionEnum.EndTurn);
        }

        EnemyAction action;
        if((action = base.DetermineAction()).actionType == ActionEnum.EndTurn){
            return new EnemyAction(ActionEnum.EndTurn);
        }

        // Prechecks
        int prechk = PreCheck();
        if (prechk != -1){
            currentTurn++;
            return new EnemyAction(ActionEnum.PlayCard, prechk);
        }

        // Turn 1
        if(currentTurn == 0){
            int turn1 = Turn1();
            if(turn1 != -1){
                // Run that card
                currentTurn++;
                return new EnemyAction(ActionEnum.PlayCard, turn1);
            } else {
                currentTurn++;
                return base.DetermineAction();
            }
        }
        // Turn 2
        if(currentTurn == 1){
            int turn2 = Turn2();
            if(turn2 != -1){
                currentTurn++;
                return new EnemyAction(ActionEnum.PlayCard, turn2);
            } else {
                currentTurn++;
                return base.DetermineAction();
            }
        }
        // Turn 3
        if(currentTurn == 2){
            int turn3 = Turn3();
            if(turn3 != -1){
                currentTurn++;
                return new EnemyAction(ActionEnum.PlayCard, turn3);
            } else {
                currentTurn++;
                return base.DetermineAction();
            }
        }

        // Turn 4
        if(currentTurn == 3){
            int turn4 = Turn4();
            if(turn4 != -1){
                currentTurn++;
                return new EnemyAction(ActionEnum.PlayCard, turn4);
            } else {
                currentTurn++;
                return base.DetermineAction();
            }
        }

        return new EnemyAction(ActionEnum.EndTurn);
    }

    // Runs the entire script for the handSize, does not do UI currently
    public void DoEverythingForMe(){
        OnStartTurn();
        for(int i = 0; i < handSize; i++){
            DetermineAction();
            EnemyAction action;
            if((action = DetermineAction()).actionType == ActionEnum.EndTurn){
                break;
            }
        }
    }

}