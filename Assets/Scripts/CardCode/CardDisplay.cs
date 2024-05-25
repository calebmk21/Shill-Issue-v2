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
    public Image displayImage;

    public void UpdateCardDisplay(){
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.descriptionText;
        displayImage.sprite = cardData.cardImage;

        //Update Type Image
        // Not curretly update the image properly. Needs to be fixed
        for (int i = 0; i < typeImage.Length; i++){
            if(i < cardData.cardType.Count){
                typeImage[i].gameObject.SetActive(true);
            } else{
                typeImage[i].gameObject.SetActive(false);
            }
        }
    }
}
