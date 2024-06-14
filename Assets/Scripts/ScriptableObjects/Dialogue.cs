using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string characterName;
        public string dialogueText;
        public Sprite dialogueImage1;
        public Sprite dialogueImage2;
    }
    //Modificar con la lista creada por mí
    public List<DialogueEntry> dialogueEntries;
}
