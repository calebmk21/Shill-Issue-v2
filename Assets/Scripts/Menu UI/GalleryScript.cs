using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShillIssue;

public class GalleryScript : MonoBehaviour
{
    [Header("Panels")] 
    public GameObject baseMenu;
    public GameObject cardFace;
    public GameObject currentPanel;

    [Header("Test")] 
    [SerializeField] public bool testBool;
    

    // Start is called before the first frame update
    void Awake()
    {
        //currentPanel = GameObject.FindGameObjectWithTag("CardPanel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
