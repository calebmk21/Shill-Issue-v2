using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTransform;
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

    private BoxCollider2D playerAreaCollider;
    private BoxCollider2D enemyAreaCollider;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;

        // Initialize original sibling index
        originalSiblingIndex = rectTransform.GetSiblingIndex();

        updateCardPlayPosition();
        updatePlayPosition();

        // Find the player and enemy areas by their tags and get their colliders
        GameObject playerArea = GameObject.FindGameObjectWithTag("Player");
        GameObject enemyArea = GameObject.FindGameObjectWithTag("Enemy");

        if (playerArea != null)
        {
            playerAreaCollider = playerArea.GetComponent<BoxCollider2D>();
            Debug.Log("Player Area found");
        }
        else
        {
            Debug.LogError("Player area not found!");
        }

        if (enemyArea != null)
        {
            enemyAreaCollider = enemyArea.GetComponent<BoxCollider2D>();
            Debug.Log("Enemy Area found");
        }
        else
        {
            Debug.LogError("Enemy area not found!");
        }
    }

    void Update()
    {
        if (needUpdateCardPlayPosition)
        {
            updateCardPlayPosition();
        }

        if (needUpdatePlayPosition)
        {
            updatePlayPosition();
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
                    OnMouseUp();
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

    public void OnPointerDown(PointerEventData eventData)
    {
        currentState = 2;

        // Save the original sibling index (if not already saved)
        if (currentState == 0)
        {
            originalSiblingIndex = rectTransform.GetSiblingIndex();
        }

        // Move the card to the front layer
        rectTransform.SetAsLastSibling();
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandlePlayState()
    {
        rectTransform.localRotation = Quaternion.identity;

        // Handle the selection state while mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            currentState = 3;
            playArrow.SetActive(true);
        }
    }

    private void updateCardPlayPosition()
    {
        if (cardPlayDivider != 0 && canvasRectTransform != null)
        {
            float segment = cardPlayMultiplier / cardPlayDivider;
            cardPlay.y = canvasRectTransform.rect.height * segment;
        }
    }

    private void updatePlayPosition()
    {
        if (canvasRectTransform != null && playPositionYDivider != 0 && playPositionXDivider != 0)
        {
            float segmentX = playPositionXMultiplier / playPositionXDivider;
            float segmentY = playPositionYMultiplier / playPositionYDivider;

            // Set the play position based on the multipliers and dividers
            playPosition.x = canvasRectTransform.rect.width * segmentX;
            playPosition.y = canvasRectTransform.rect.height * segmentY;
        }
    }

    private void OnMouseUp()
    {
        currentState = 0;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if the mouse is released in the player area
        if (IsMouseInArea(playerAreaCollider, mousePosition))
        {
            Debug.Log("Card dropped on player area");
            // Handle card played on player
        }
        // Check if the mouse is released in the enemy area
        else if (IsMouseInArea(enemyAreaCollider, mousePosition))
        {
            Debug.Log("Card dropped on enemy area");
            // Handle card played on enemy
        }
        else
        {
            // Reset position if not dropped in any area
            rectTransform.localPosition = originalPosition;
            Debug.Log("Made it to the else statement :/");  
        }
        // Reset the card's sibling index to its original value
        rectTransform.SetSiblingIndex(originalSiblingIndex);
        playArrow.SetActive(false); // Disable playArrow
    }

    private bool IsMouseInArea(BoxCollider2D areaCollider, Vector2 mousePosition)
    {
        if (areaCollider == null)
        {
            Debug.LogError("Area collider is missing!");
            return false;
        }

        bool isInArea = areaCollider.OverlapPoint(mousePosition);
        Debug.Log($"Checking if mouse is in area: {isInArea}, Mouse Position: {mousePosition}");
        return isInArea;
    }
}
