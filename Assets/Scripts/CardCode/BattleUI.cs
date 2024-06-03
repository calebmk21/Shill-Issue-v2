using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    //public GameObject playerTitle;
    //public GameObject enemyTitle;

    public GameObject playerTurn;
    public GameObject enemyTurn;

    void Update()
    {
        if (GameManager._instance.currentBattle)
        {
            //playerTitle.SetText("Em");
            //if (GameManager._instance.currentBattle.battleOpponent != null)
            //{
            //    enemyTitle.SetText(GameManager._instance.currentBattle.battleOpponent.name);
            //}

            if (GameManager._instance.currentBattle.playerTurn)
            {
                playerTurn.SetActive(true);
                enemyTurn.SetActive(false);
            }
            else
            {
                playerTurn.SetActive(false);
                enemyTurn.SetActive(true);
            }
        }
    }
}
