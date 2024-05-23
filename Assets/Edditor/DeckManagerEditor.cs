using System.Collections;
using System.Collections.Generic;
using UnityEditor;


#if UNITY_EDITOR

using UnityEngine;
[CustomEditor(typeof(DeckManager))]

public class DeckManagerEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        DeckManager deckManager = (DeckManager)target;
        if(GUILayout.Button("Draw Next Card")){
            HandManager handManager = FindObjectOfType<HandManager>();
            if(handManager != null){
                deckManager.DrawCard(handManager);
            }
        }
    }
}
#endif