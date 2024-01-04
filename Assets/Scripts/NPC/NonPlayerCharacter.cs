using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour,ICanTalkWith
{
    public DialogText dialogText;
    public string[] textContent;

    // Start is called before the first frame update
    void Start()
    {
        textContent = dialogText.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDialogUI()
    {
        UIManager.Instance.OpenDialog(textContent);
    }

    public void NextLine() 
    {
        UIManager.Instance.NextDialogLine();
    }
}
