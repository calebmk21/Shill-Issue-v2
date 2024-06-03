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
    public Image characterSprite;
    public Image backgroundSprite;
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

    public void PauseGame() {
        Time.timeScale = 0f;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
    }

    [YarnCommand("show_sprite")]
    public void Show() {
        characterSprite.enabled = true;
    }

    [YarnCommand("hide_sprite")]
    public void Hide() {
        characterSprite.enabled = false; 
    }

    [YarnCommand("show_background")]
    public void ShowBackground() {
        backgroundSprite.enabled = true;
    }

    [YarnCommand("hide_background")]
    public void HideBackground() {
        backgroundSprite.enabled = false; 
    }

    // use <<change_character characterName mood>>
    [YarnCommand("change_character")]
    public void ChangeCharacter(string characterName, string mood)
    {
        Vector2 spriteSize = Vector2.zero; 

        if (characters.ContainsKey(characterName))
        {
            if (mood == "neutral")
            {
                characterSprite.sprite = characters[characterName].neutral;
                spriteSize = characters[characterName].neutral.bounds.size;
            }
            else if (mood == "happy")
            {
                characterSprite.sprite = characters[characterName].happy;
                spriteSize = characters[characterName].happy.bounds.size;
            }
            else if (mood == "sad")
            {
                characterSprite.sprite = characters[characterName].sad;
                spriteSize = characters[characterName].sad.bounds.size;
            }
            else if (mood == "angry")
            {
                characterSprite.sprite = characters[characterName].angry;
                spriteSize = characters[characterName].angry.bounds.size;
            }
            else
            {
                Debug.LogError("Mood not found for character: " + characterName);
            }

            // resize
            RectTransform rectTransform = characterSprite.GetComponent<RectTransform>();
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
