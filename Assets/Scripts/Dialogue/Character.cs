using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[CreateAssetMenu(fileName = "New Character Data", menuName = "Dialogue System/Character Data")]
public class Character : ScriptableObject
{
    public string fullName;
    public Sprite neutral;
    public Sprite happy;
    public Sprite sad;
    public Sprite angry; 
}
