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
        public List<CardType> cardType;
        
        // If we ant to do Summons We could give them health
        //public int health;
        
        public int damageMin;
        public int damageMax;
        public List<AttackType> attackType;
        // Card Sprite
        public Sprite cardImage;
        public GameObject perfab;

    }

    public enum CardType
    {
        //heal player
        Heal,
        //damage enemy
        Dmg
    }

    public enum AttackType
    {
        Instant,
        OverTime
    }

}