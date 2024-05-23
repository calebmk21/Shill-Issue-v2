using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using ShillIssue;
using Unity.VisualScripting;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image[] typeImage;
    //public TMP_Text damageText;

    void Start(){
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay(){
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.descriptionText;
        //damageText.text = $"{cardData.damageMin} - {cardData.damageMax}";

        //Update Type Image
        for (int i = 0; i < typeImage.Length; i++){
            if(i < cardData.cardType.Count){
                typeImage[i].gameObject.SetActive(true);
            } else{
                //typeImage[i].gameObject.SetActive(false);
            }
        }
    }
}
