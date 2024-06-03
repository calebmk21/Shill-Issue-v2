using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem; 
using Yarn.Unity; 
using UnityEngine.UI;

public class VisualNovel : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    public Dictionary<string, Character> characters = new Dictionary<string, Character>();
    public Image component;
    public Character diego; 
    public Character emCee;
    public Character umeko;
    public Character yuCee; 

    private void Awake() {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();  
        characters.Add(diego.fullName, diego);
        characters.Add(emCee.fullName, emCee);
        characters.Add(umeko.fullName, umeko);
        characters.Add(yuCee.fullName, yuCee);
    }

    public void StartDialogue(string node) {
        dialogueRunner.StartDialogue(node); 
    }

    [YarnCommand("show_sprite")]
    public void Show() {
        component.enabled = true;
    }

    [YarnCommand("hide_sprite")]
    public void Hide() {
        component.enabled = false; 
    }

    // use <<change_character characterName mood>>
    [YarnCommand("change_character")]
    public void ChangeCharacter(string characterName, string mood)
    {
        // Debug.Log(parameters);
        // if (parameters.Length != 2)
        // {
        //     Debug.LogError("error: 2 params only");
        //     return;
        // }

        // string characterName = parameters[0];
        // string mood = parameters[1];
        Vector2 spriteSize = Vector2.zero; 

        if (characters.ContainsKey(characterName))
        {
            if (mood == "neutral")
            {
                component.sprite = characters[characterName].neutral;
                spriteSize = characters[characterName].neutral.bounds.size;
            }
            else if (mood == "happy")
            {
                component.sprite = characters[characterName].happy;
                spriteSize = characters[characterName].happy.bounds.size;
            }
            else if (mood == "sad")
            {
                component.sprite = characters[characterName].sad;
                spriteSize = characters[characterName].sad.bounds.size;
            }
            else if (mood == "angry")
            {
                component.sprite = characters[characterName].angry;
                spriteSize = characters[characterName].angry.bounds.size;
            }
            else
            {
                Debug.LogError("Mood not found for character: " + characterName);
            }

            // resize
            RectTransform rectTransform = component.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = spriteSize * 300;
            }
            else
            {
                Debug.LogError("RectTransform not found on Image component!");
            }
        }
        else
        {
            Debug.LogError("Character not found: " + characterName);
        }
    }

    // // moves camera to camera location in scene
    // private void ChangeCameraLocation(Location location) {
    //     Camera.main.transform.position = location.cameraMarker.position;
    //     Camera.main.transform.rotation = location.cameraMarker.rotation; 
    // }

    // private void PlaceCharacter(string characterName, string markerName) {
    //     Character character;

    //     // if character has not been instantiated before, do so now
    //     if (!characters.ContainsKey(characterName)) {
    //         var characterPrefab = characterList.FindCharacterPrefab(characterName);
    //         // and place it in the list of characters so we can find it next time
    //         characters[characterName] = character;
    //     }
    //     else {
    //         character = characters[characterName]; 
    //     }

    //     // get pos/rotation fo destination market in scene
    //     // and set position/rotation of character to tehre
    //     var marker = GameObject.Find(markerName);
    //     character.transform.position = marker.transform.position;
    //     character.transform.rotation = marker.transform.rotation; 
    // }
}
