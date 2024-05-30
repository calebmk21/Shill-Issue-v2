using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace ShillIssue
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public string descriptionText;
        public GameManager.CardEnum cardEnum;
        public List<CardType> cardType;

        public bool targetSelf = false;
        
        // If we ant to do Summons We could give them health
        //public int health;
        
        public int damageMin;
        public int damageMax;

        public int healMin;
        public int healMax;

        public StatusEffect statusEffect;

        public List<AttackType> attackType;
        // Card Sprite
        public Sprite cardImage;
        public GameObject perfab;

    }

    [System.Serializable]
    public struct StatusEffect
    {
        public StatusType statusType;
        public int statusNum;
    }

    public enum CardType
    {
        //heal player
        Heal,
        //damage enemy
        Dmg,
        Status
    }

    public enum AttackType
    {
        Instant,
        OverTime
    }

    public enum StatusType
    {
        Strength,
        Vulnerable
    }

}