using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler //IPointerExitHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTranform;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private int originalSiblingIndex;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private int cardPlayDivider = 4;
    [SerializeField] private float cardPlayMultiplier = 1f;
    [SerializeField] private bool needUpdateCardPlayPosition = false;
    [SerializeField] private int playPositionYDivider = 8;
    [SerializeField] private float playPositionYMultiplier = 1f;
    [SerializeField] private int playPositionXDivider = 4;
    [SerializeField] private float playPositionXMultiplier = 1f;
    [SerializeField] private bool needUpdatePlayPosition = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRectTranform = canvas.GetComponent<RectTransform>();
        }

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;

        // Initialize original sibling index
        originalSiblingIndex = rectTransform.GetSiblingIndex();

        updateCardPlayPostion();
        updatePlayPostion();
    }

    void Update()
    {
        if (needUpdateCardPlayPosition)
        {
            updateCardPlayPostion();
        }

        if (needUpdatePlayPosition)
        {
            updatePlayPostion();
        }

        switch (currentState)
        {
            case 1:
                HandleHoverState();
                TransitionToState0();
                break;
            case 2:
                HandlePlayState();
                break;
            case 3:
                if (!Input.GetMouseButton(0)) // Check if mouse button is released
                {
                    TransitionToState0();
                }
                break;
        }
    }


    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale; // Reset Scale
        rectTransform.localRotation = originalRotation; // Reset Rotation
        rectTransform.localPosition = originalPosition; // Reset Position
        glowEffect.SetActive(false); // Disable glow effect
        playArrow.SetActive(false); // Disable playArrow

        // Reset the card's sibling index to its original value
        rectTransform.SetSiblingIndex(originalSiblingIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
        }
    }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (currentState == 1)
    //     {
    //         TransitionToState0();
    //     }
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Remove this if statement
        // if (currentState == 1)
        // {
            currentState = 2;

            // Save the original sibling index (if not already saved)
            if (currentState == 0)
            {
                originalSiblingIndex = rectTransform.GetSiblingIndex();
            }

            // Move the card to the front layer
            rectTransform.SetAsLastSibling();
        // }
    }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     if (currentState == 2)
    //     {
    //         if (Input.mousePosition.y > cardPlay.y)
    //         {
    //             currentState = 3;
    //             playArrow.SetActive(true);

    //             // Adjust this line to snap to the desired position
    //             rectTransform.localPosition = playPosition; 
    //         }
    //     }
    // }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    // private void HandleDragState()
    // {
    //     // Set the card's rotation to zero
    //     rectTransform.localRotation = Quaternion.identity;
    //     rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);
    // }

    private void HandlePlayState()
    {
        rectTransform.localRotation = Quaternion.identity;

        // Handle the selection state while mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            currentState = 3;
            playArrow.SetActive(true);

            // Adjust this line to snap to the desired position
            // rectTransform.localPosition = playPosition; 
        }
    }


    private void updateCardPlayPostion()
    {
        if (cardPlayDivider != 0 && canvasRectTranform != null)
        {
            float segment = cardPlayMultiplier / cardPlayDivider;
            cardPlay.y = canvasRectTranform.rect.height * segment;
        }
    }

    private void updatePlayPostion()
    {
        if (canvasRectTranform != null && playPositionYDivider != 0 && playPositionXDivider != 0)
        {
            float segmentX = playPositionXMultiplier / playPositionXDivider;
            float segmentY = playPositionYMultiplier / playPositionYDivider;

            // Set the play position based on the multipliers and dividers
            playPosition.x = canvasRectTranform.rect.width * segmentX;
            playPosition.y = canvasRectTranform.rect.height * segmentY;
        }
    }
}
