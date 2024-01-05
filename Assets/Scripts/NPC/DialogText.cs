using UnityEngine;


[CreateAssetMenu(menuName = ("Dialog/DialogText"),fileName = ("NPCDialogText"))]

public class DialogText : ScriptableObject
{
    public string[] openText;
    public string[] loopText;
    public string[] hiddenText;
}