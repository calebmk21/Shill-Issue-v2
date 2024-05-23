using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPointerPosition;
    private Vector3 originalScale;
    private int currentState;
    private Quaternion orininalRotation;
    private Vector3 originalPosition;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;

    void Awake(){
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        orininalRotation = rectTransform.localRotation;
    }

    void Update(){
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if(!Input.GetMouseButton(0)){
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                if(!Input.GetMouseButton(0)){
                    TransitionToState0();
                }
                break;
        }
    }

    private void TransitionToState0(){
        currentState = 0;
        rectTransform.localScale = originalScale; // Resets Scale
        rectTransform.localRotation = orininalRotation; // Resets Rotaion
        rectTransform.localPosition = originalPosition; // Resets Position

        glowEffect.SetActive(false); //Disable glow effect
        playArrow.SetActive(false); // Disable playArrow
   }

   public void OnPointerEnter(PointerEventData eventData){
    if(currentState == 0){
        originalPosition = rectTransform.localPosition;
        orininalRotation = rectTransform.localRotation;
        originalScale = rectTransform.localScale;

        currentState = 1;
    }
   }

   public void OnPointerExit(PointerEventData eventData)
   {
        if(currentState == 1)
        {
            TransitionToState0();
        }
   }

   public void OnPointerDown(PointerEventData eventData)
   {
        if (currentState == 1){
            currentState = 2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPointerPosition = rectTransform.localPosition;
        }
   }

   public void OnDrag(PointerEventData eventData)
   {
        if(currentState == 2)
        {
            Vector2 localPointerPosition;
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);

                if(rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
                }

            }
        }
   }

   private void HandleHoverState()
   {
    glowEffect.SetActive(true);
    rectTransform.localScale = originalScale * selectScale;
   }

   private void HandleDragState()
   {
        // Set the cards rotaion to zero
        rectTransform.localRotation = Quaternion.identity;
   }

   private void HandlePlayState()
   {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;
        
        if(Input.mousePosition.y < cardPlay.y){
            currentState = 2;
            playArrow.SetActive(false);
        }
   }


}
