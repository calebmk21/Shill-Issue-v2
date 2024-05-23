using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.Rendering;
using UnityEngine;

public class ArcRenderer : MonoBehaviour
{
    public GameObject arrowPrefab; //The arrow head
    public GameObject dotPrefab; // the dots
    public int poolSize = 50; // size of our dot pool
    private List<GameObject> dotPool = new List<GameObject>(); // the dot pool
    private GameObject arrowInstance; // Holds a referanc eto the arrow head

    public float spacing = 50; // spacing of teh dots
    public float arrowAngleAdjustment = 0; // Angle correction for the Arrow Head
    public int dotsToSkip = 1; // Number of dots to skip to give the Arrow head space
    private Vector3 arrowDirection; // Holds the position the arrow head needs to point from
    
    void Start()
    {
        arrowInstance = Instantiate(arrowPrefab, transform);
        arrowInstance.transform.localPosition = Vector3.zero;
        InitializeDotPool(poolSize);

    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 startPos = transform.position;
        Vector3 midPos = CalculateMidPoint(startPos, mousePos);

        UpdateArc(startPos, midPos, mousePos);
        PositionAndRotateArrow(mousePos);
    }

    void UpdateArc(Vector3 start, Vector3 mid, Vector3 end)
    {
        int numDots = Mathf.CeilToInt(Vector3.Distance(start, end) / spacing);

        for(int i = 0; i < numDots && i < dotPool.Count; i++){
            float t = i / (float)numDots;
            t = Mathf.Clamp(t, 0f, 1f); //Ensure t staus within teh range [0,1]

            Vector3 posistion = QuadraticBezierPoint(start, mid, end, t);

            if (i != numDots - dotsToSkip) {
                dotPool[i].transform.position = posistion;
                dotPool[i].SetActive(true);
            }
            if(i == numDots - (dotsToSkip + 1) && i - dotsToSkip + 1 >= 0){
                arrowDirection = dotPool[i].transform.position;
            }
        }

        //Deactivate unused dots
        for(int i = numDots - dotsToSkip; i < dotPool.Count; i++){
            if (i>0){
                dotPool[i].SetActive(false);
            }
        }
    }

    void PositionAndRotateArrow(Vector3 position)
    {
        arrowInstance.transform.position = position;
        Vector3 direction = arrowDirection - position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += arrowAngleAdjustment;
        arrowInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    Vector3 CalculateMidPoint (Vector3 start, Vector3 end)
    {
        Vector3 midpoint = (start + end) / 2;
        float arcHeight = Vector3.Distance(start, end)/3f;
        midpoint.y += arcHeight;
        return midpoint;
    }

    Vector3 QuadraticBezierPoint(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        float u = 1-t;
        float tt = t*t;
        float uu = u*u;

        Vector3 point = uu * start;
        point += 2 * u * t * control;
        point += tt * end;
        return point;
    }

    void InitializeDotPool(int count)
    {
        for (int i = 0; i < count; i++){
            GameObject dot = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity, transform);
            dot.SetActive(true);
            dotPool.Add(dot);
        }
    }
}
