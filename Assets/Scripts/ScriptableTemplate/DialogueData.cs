using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public Image headicon;
    public string speakerName;
    [TextArea(3, 10)]
    public string[] sentences;

    [TextArea(3, 10)]
    public string[] loopsentences;

    [TextArea(3, 10)]
    public string[] hiddensentences;
}