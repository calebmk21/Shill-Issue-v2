using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ShillIssue;
using System;

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

    public Enemy battleOpponent;

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

    // UI references
    public Slider healthSlider;
    public Slider manaSlider;

    public SpriteRenderer playerSpriteRenderer;
    public SpriteRenderer enemySpriteRenderer;

    public Sprite playerNormalSprite;
    public Sprite playerDamageSprite;
    public Sprite playerWinSprite;
    public Sprite playerLoseSprite;

    public Sprite enemyNormalSprite;
    public Sprite enemyDamageSprite;
    public Sprite enemyWinSprite;
    public Sprite enemyLoseSprite;

    private Coroutine playerSpriteCoroutine;
    private Coroutine enemySpriteCoroutine;


    void Start()
    {
        GameManager._instance.currentBattle = this;
        ChangeGameplayState(GameplayState.NOT_PLAYING);

        object[] constructorArgs = new object[] { this };
        battleOpponent = (Enemy)Activator.CreateInstance(GameManager._instance.enemyDict[GameManager._instance.currentEnemy].enemyType, constructorArgs);

        SetEnemySprite(GameManager._instance.enemyDict[GameManager._instance.currentEnemy].enemySprite);

        UpdateResourceUI();

        StartBattle();

        //onChangePlayerHealth += UpdateHealthSlider;
        //onChangePlayerMana += UpdateManaSlider;

        //// Initialize sliders
        //healthSlider.maxValue = maxHealth;
        //manaSlider.maxValue = maxMana;
        //UpdateHealthSlider(currentHealth, maxHealth);
        //UpdateManaSlider(currentMana, maxMana);

        //// Find all game objects with the "Enemy" tag
        //GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        //// Iterate through the enemy objects and get their Enemy components
        //foreach (GameObject enemyObject in enemyObjects)
        //{
        //    Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
        //    if (enemyComponent != null)
        //    {
        //        enemies.Add(enemyComponent);
        //    }
        //    else
        //    {
        //        Debug.LogError("Enemy component not found on an object with the 'Enemy' tag!");
        //    }
        //}


    }

     void OnDestroy()
    {
        //onChangePlayerHealth -= UpdateHealthSlider;
        //onChangePlayerMana -= UpdateManaSlider;
    }

    void Update()
    {    
    }

    public void SetPlayerSprite(Sprite sprite)
    {
        playerSpriteRenderer.sprite = sprite;
    }

    public void SetEnemySprite(Sprite sprite)
    {
        enemySpriteRenderer.sprite = sprite;
    }

    public void ChangePlayerSprite(Sprite sprite, float duration)
    {
        if (playerSpriteCoroutine != null)
        {
            StopCoroutine(playerSpriteCoroutine);
        }

        playerSpriteCoroutine = StartCoroutine(ChangePlayerSpriteCoroutine(sprite, duration));
    }

    public void ChangeEnemySprite(Sprite sprite, float duration)
    {
        if (enemySpriteCoroutine != null)
        {
            StopCoroutine(enemySpriteCoroutine);
        }

        enemySpriteCoroutine = StartCoroutine(ChangeEnemySpriteCoroutine(sprite, duration));
    }

    private IEnumerator ChangePlayerSpriteCoroutine(Sprite sprite, float duration)
    {
        SetPlayerSprite(sprite);
        yield return new WaitForSeconds(duration);
        SetPlayerSprite(playerNormalSprite); // Change back to normal sprite
    }

    private IEnumerator ChangeEnemySpriteCoroutine(Sprite sprite, float duration)
    {
        SetEnemySprite(sprite);
        yield return new WaitForSeconds(duration);
        SetEnemySprite(enemyNormalSprite); // Change back to normal sprite
    }

    public void UpdateResourceUI()
    {
        onChangePlayerHealth?.Invoke(currentHealth, maxHealth);
        onChangeEnemyHealth?.Invoke(battleOpponent.currentHealth, battleOpponent.maxHealth);
        onChangePlayerMana?.Invoke(currentMana, maxMana);
        onChangeEnemyMana?.Invoke(battleOpponent.currentMana, battleOpponent.maxMana);
    }

    //private void UpdateHealthSlider(float currHealth, float maxHealth)
    //{
    //    healthSlider.value = currHealth;
    //}

    //private void UpdateManaSlider(float currMana, float maxMana)
    //{
    //    manaSlider.value = currMana;
    //}

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
                // Change player sprite to damage sprite
                ChangePlayerSprite(playerDamageSprite, 1.0f); // Change sprite for 1 second
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

    
    protected void ChangeGameplayState(GameplayState newState)
    {
        gameplayState = newState;
        onChangeState?.Invoke(gameplayState);
    }

    public void Die()
    {
        // Add death logic here
        ChangePlayerSprite(playerLoseSprite, 3.0f); // Change sprite for 3 seconds
    }

    public void Win()
    {
        // Add win logic here
        ChangePlayerSprite(playerWinSprite, 3.0f); // Change sprite for s seconds
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

    public void AddStatus(ShillIssue.StatusEffect status, Enemy enemy = null){
        if (enemy == null){
            bool foundStatus = false;

            for (int i = 0 ; i < statuses.Count ; i++){
                if (statuses[i].statusType == status.statusType){
                    statuses[i].statusNum += status.statusNum;
                    foundStatus = true;
                    break;
                }
            }

            if (!foundStatus){
                statuses.Add(new ShillIssue.StatusEffect(status.statusType, status.statusNum, status.onOpponent));
            }
        }
        else{
            bool foundStatus = false;

            for (int i = 0 ; i < statuses.Count ; i++){
                if (enemy.statuses[i].statusType == status.statusType){
                    enemy.statuses[i].statusNum += status.statusNum;
                    foundStatus = true;
                    break;
                }
            }

            if (!foundStatus){
                enemy.statuses.Add(new ShillIssue.StatusEffect(status.statusType, status.statusNum, status.onOpponent));
            }
        }
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
        battleOpponent.DoTurn();

        StartTurn();
    }

    public bool IsPlayable(ShillIssue.Card card)
    {
        return card.damageMax <= currentMana;
    }

    //public void HandleCardPlayedOnPlayer(ShillIssue.Card card)
    //{
    //    // Implement your logic to handle the card being played on the player
    //    Debug.Log("HandleCardPlayedOnPlayer called");
    //    PlayCard(card, -1); // Assuming -1 means it's not from the hand
    //}

    //public void HandleCardPlayedOnEnemy(ShillIssue.Card card)
    //{
    //    // Implement your logic to handle the card being played on the enemy
    //    Debug.Log("HandleCardPlayedOnEnemy called");

    //    // Find the enemy target by tag
    //    GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
    //    if (enemyObject != null)
    //    {
    //        Enemy enemyTarget = enemyObject.GetComponent<Enemy>();
    //        if (enemyTarget != null)
    //        {
    //            PlayCard(card, -1, enemyTarget); // Pass the enemy target
    //        }
    //        else
    //        {
    //            Debug.LogError("Enemy component not found on the enemy object!");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("No enemy object found with the tag 'Enemy'!");
    //    }
    //}




    public bool PlayCard(ShillIssue.Card card, Enemy enemy = null)
    {
        if (!IsPlayable(card)) { return false; }

        // Determine the target based on whether enemy is null or not
        Enemy selfTarget = enemy == null ? null : battleOpponent;
        Enemy enemyTarget = enemy == null ? battleOpponent : null;

        foreach (ShillIssue.CardType cardType in card.cardType)
        {
            switch (cardType)
            {
                case ShillIssue.CardType.Dmg:
                    float damageNum = UnityEngine.Random.Range(card.damageMin, card.damageMax + 1);
                    if (ContainsStatus(ShillIssue.StatusType.Strength, selfTarget))
                    {
                        damageNum *= 2;
                    }
                    if (ContainsStatus(ShillIssue.StatusType.Vulnerable, enemyTarget))
                    {
                        damageNum *= 2;
                    }
                    ChangeHealth(damageNum, enemyTarget);
                    break;
                case ShillIssue.CardType.Heal:
                    ChangeHealth(UnityEngine.Random.Range(card.healMin, card.healMax + 1), selfTarget);
                    break;
                case ShillIssue.CardType.Status:
                    for (int i = 0; i < card.statusEffect.Count; i++)
                    {
                        if (card.statusEffect[i].onOpponent)
                        {
                            AddStatus(card.statusEffect[i], enemyTarget);
                        }
                        else
                        {
                            AddStatus(card.statusEffect[i], selfTarget);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        return true;
    }


}
