using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string speakerName;
    [TextArea(3, 10)]
    public string[] sentences;
}