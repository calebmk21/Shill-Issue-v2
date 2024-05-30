using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public enum GameplayState { NOT_PLAYING, PLAYING };

    public GameplayState gameplayState = GameplayState.NOT_PLAYING;

    public bool pauseGame = false;

    // player stats

    public float currentHealth = 100;
    public float maxHealth = 100;

    public float currentMana = 0;
    public float maxMana = 100f;

    public int handSize = 5;
    public float manaGain = 10f;

    public List<ShillIssue.StatusEffect> statuses = new List<ShillIssue.StatusEffect>();

    // gameplay stuffs

    public List<Enemy> enemies = new List<Enemy>();

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
        onChangePlayerHealth?.Invoke(currentHealth, maxHealth);
        //onChangeEnemyHealth?.Invoke(playerHealth, playerMaxHealth);
        onChangePlayerMana?.Invoke(currentMana, maxMana);
        //onChangeEnemyMana?.Invoke(playerHealth, playerMaxHealth);
    }

    public void ChangeMana(float amt, Enemy enemy = null)
    {
        if (enemy == null)
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
            onChangePlayerMana?.Invoke(currentMana, maxMana);
        }
        else
        {
            enemy.currentMana += amt;

            if (enemy.currentMana > enemy.maxMana)
            {
                enemy.currentMana = enemy.maxMana;
            }
            else if (enemy.currentMana < 0)
            {
                enemy.currentMana = 0;
            }
            onChangeEnemyMana?.Invoke(enemy.currentMana, enemy.maxMana);
        }
    }

    public void ChangeHealth(float amt, Enemy enemy = null)
    {

        if (enemy == null)
        {
            currentHealth += amt;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (currentHealth < 0)
            {
                currentHealth = 0;
                Die();
            }
            onChangePlayerHealth?.Invoke(currentHealth, maxHealth);
        }
        else
        {
            enemy.currentHealth += amt;

            if (enemy.currentHealth > enemy.maxHealth)
            {
                enemy.currentHealth = enemy.maxHealth;
            }
            else if (enemy.currentHealth < 0)
            {
                enemy.currentHealth = 0;
                enemy.Die();
            }
            onChangeEnemyHealth?.Invoke(enemy.currentHealth, enemy.maxHealth);
        }
    }
    
    private void ChangeGameplayState(GameplayState newState)
    {
        gameplayState = newState;
        onChangeState?.Invoke(gameplayState);
    }

    public void Die()
    {
        ;
    }

    public void Win()
    {
        ;
    }

    public void StartBattle()
    {
        ChangeGameplayState(GameplayState.PLAYING);
        foreach (ShillIssue.Card card in GameManager._instance.deck)
        {
            drawPile.Add(card);
        }

        StartTurn();
    }

    public void ShuffleDiscard(Enemy enemy = null)
    {
        if (enemy == null)
        {
            foreach (ShillIssue.Card card in discardPile)
            {
                drawPile.Add(card);
            }

            drawPile.Shuffle();

            discardPile.Clear();
        }
        else
        {
            foreach (ShillIssue.Card card in enemy.discardPile)
            {
                enemy.drawPile.Add(card);
            }

            enemy.drawPile.Shuffle();

            enemy.discardPile.Clear();
        }
    }

    public void DiscardCard(int index, Enemy enemy = null)
    {
        if (enemy == null)
        {
            onDiscardCard?.Invoke(hand[index]);
            discardPile.Add(hand[index]);
            hand.RemoveAt(index);
        }
        else
        {
            onDiscardCard?.Invoke(hand[index]);
            enemy.discardPile.Add(hand[index]);
            enemy.hand.RemoveAt(index);
        }
    }

    public void DrawCards(int num, Enemy enemy = null)
    {
        if (enemy == null)
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
        else
        {
            for (int i = 0; i < num; i++)
            {
                if (enemy.drawPile.Count <= 0)
                {
                    ShuffleDiscard(enemy);
                    if (enemy.drawPile.Count == 0)
                    {
                        return;
                    }
                }
                ShillIssue.Card card = enemy.drawPile[0];
                enemy.drawPile.RemoveAt(0);
                enemy.hand.Add(card);
            }
        }
    }

    public void StartTurn()
    {
        ChangeMana(manaGain);
        DrawCards(handSize - hand.Count);
    }
    public void EndTurn()
    {
        DecrementStatuses();
        EnemyTurn();
    }

    public void DecrementStatuses(Enemy enemy = null)
    {
        if (enemy == null)
        {
            for (int i = statuses.Count - 1; i > -1; i--)
            {
                ShillIssue.StatusEffect newStatus = statuses[i];
                newStatus.statusNum = newStatus.statusNum - 1;

                if (newStatus.statusNum <= 0)
                {
                    statuses.RemoveAt(i);
                }
                else
                {
                    statuses[i] = newStatus;
                }
            }
        }
        else
        {
            for (int i = enemy.statuses.Count - 1; i > -1; i--)
            {
                ShillIssue.StatusEffect newStatus = enemy.statuses[i];
                newStatus.statusNum = newStatus.statusNum - 1;

                if (newStatus.statusNum <= 0)
                {
                    enemy.statuses.RemoveAt(i);
                }
                else
                {
                    enemy.statuses[i] = newStatus;
                }
            }
        }
    }

    public bool ContainsStatus(ShillIssue.StatusType statusType, Enemy enemy = null)
    {
        if (enemy == null)
        {
            foreach (ShillIssue.StatusEffect status in statuses)
            {
                if (status.statusType == statusType)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            foreach (ShillIssue.StatusEffect status in enemy.statuses)
            {
                if (status.statusType == statusType)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void EnemyTurn()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.DoTurn();
        }

        StartTurn();
    }

    public bool IsPlayable(ShillIssue.Card card)
    {
        return card.damageMax <= currentMana;
    }

    public bool PlayCard(ShillIssue.Card card, int index, Enemy enemy = null)
    {
        if (!IsPlayable(card)) { return false; }

        Enemy selfTarget = enemy == null ? null : enemies[0];
        Enemy enemyTarget = enemy == null ? enemies[0] : null;

        foreach (ShillIssue.CardType cardType in card.cardType)
        {
            switch (cardType)
            {
                case ShillIssue.CardType.Dmg:
                    float damageNum = Random.Range(card.damageMin, card.damageMax + 1);
                    if (ContainsStatus(ShillIssue.StatusType.Strength, selfTarget))
                    {
                        damageNum *= 2;
                    }
                    if (ContainsStatus(ShillIssue.StatusType.Vulnerable, enemyTarget)){
                        damageNum *= 2;
                    }
                    ChangeHealth(damageNum, enemyTarget);
                    break;
                case ShillIssue.CardType.Heal:
                    ChangeHealth(Random.Range(card.healMin, card.healMax + 1), selfTarget); // change to heal value;
                    break;
                case ShillIssue.CardType.Status:
                    break;
                default:
                    break;
            }
        }

        DiscardCard(index, selfTarget);
        return true;
    }
}
