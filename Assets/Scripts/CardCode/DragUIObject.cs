using UnityEngine;
using UnityEngine.EventSystems; 

public class DragUIObject : MonoBehaviour, IDragHandler, IPointerDownHandler //These classes hold the methods required to handle UI interactions
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    public float movementSensitivity = 1.0f; // Adjustable sensitivity if needed

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); 
    }

    public void OnPointerDown(PointerEventData eventData) //This is inherited from the IPointerDownHandler class referenced above
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition); //Using the event system to detect what is clicked on
        originalPanelLocalPosition = rectTransform.localPosition;
    }

    public void OnDrag(PointerEventData eventData) //This is inherited from the IDragHandler class referenced above
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition))
        {
            localPointerPosition /= canvas.scaleFactor;

            // Adjusting the movement based on sensitivity
            Vector3 offsetToOriginal = (localPointerPosition - originalLocalPointerPosition) * movementSensitivity;
            rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;

            // Debug output
            Debug.Log($"Drag - LocalPointerPosition: {localPointerPosition}, Offset: {offsetToOriginal}, New Position: {rectTransform.localPosition}"); //Comment out this line if not debugging an issue, otherwise it will flood the console unnecessarily
        }
    }
}
