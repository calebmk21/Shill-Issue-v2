using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Callbacks;
using UnityEngine;

public class ExampleEnemy2 : Enemy {
    // Cards left this turn
    public int cardsLeft = 5;
    // From 0-3
    public int currentTurn = 0;
    // This is independent of the number of cards you can play per turn
    public int numTurns = 4;
    // Is this the very first time the enemy has drawn a card?
    public bool firstTurn = false;

    public ExampleEnemy2(Battle _battle) : base(_battle){
        name = "Example enemy 2";
        enemyEnum = EnemyEnum.ExampleEnemy;
        cardsLeft = 5;
        currentTurn = 0;
        firstTurn = false;

        // Add cards to hand
        for(int i = 0; i < handSize; i++){
            AddCards();
        }
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
    // "Heal" enemy
    // -1 implies this card doesn't exist
    public int PreCheck(){
        // Get highest heal card
        int index = GetHighestHealCard();
        ShillIssue.Card card;
        if(index != -1){
            card = hand[index];
        } else {
            return -1;
        }

        float healAmount = (card.healMax + card.healMin) / 2f;

        // Check if player has < 50% of health
        if(currentHealth < 50){
            return index;
        }

        return -1;
    }

    // Return -1 if no card found
    public int Turn1() {
        int index = GetLargestManaCard();
        if(index != -1){
            return index;
        }
        return -1;
    }

    // Return -1 if no card found
    public int Turn2() {
        int index = GetLowestHealCard();
        if(index != -1){
            return index;
        }
        return -1;
    }

    // Return -1 if no card found
    public int Turn3() {
        int index = GetLargestManaCard();
        if(index != -1){
            return index;
        }
        return -1;
    }

    // Return -1 if no card found
    public int Turn4() {
        int index = GetLargestAttackCard();
        if(index != -1){
            return index;
        }
        return -1;
    }

    public override EnemyAction DetermineAction(){
        // Calculate turn
        currentTurn = currentTurn % numTurns;
        // Edge cases
        if((cardsLeft <= 0) && (hand.Count != 0)){
            return new EnemyAction(ActionEnum.EndTurn);
        }

        EnemyAction action;
        if((action = DetermineAction()).actionType == ActionEnum.EndTurn){
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
                return new EnemyAction(ActionEnum.DiscardCard, turn3);
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
        if(!firstTurn){
            OnStartTurn();
        } else {
            firstTurn = false;
        }
        for(int i = 0; i < handSize; i++){
            DetermineAction();
            EnemyAction action;
            if((action = DetermineAction()).actionType == ActionEnum.EndTurn){
                break;
            }
        }
    }
}