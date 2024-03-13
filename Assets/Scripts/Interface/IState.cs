using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

// ×´Ì¬½Ó¿Ú
public interface IState
{
    void HandleEKeyPress(ControlManager manager);
}

public class DialogState : IState
{
    DialogueUI dialogueUI;
    DialogueData curData;
    public DialogState(DialogueData data) 
    {
        curData = data;
    }


    public void HandleEKeyPress(ControlManager manager)
    {
        if (UIManager.Instance.isUIOpen("DialoguePanel"))
        {
            dialogueUI.KeyDownWay();
        }
        else 
        {
            UIManager.Instance.OpenUI("DialoguePanel");
            dialogueUI = (DialogueUI)UIManager.Instance.GetUIPanel("DialoguePanel");
            dialogueUI.StartDialogue(curData);
        }     
    }
}

public class ExploreState : IState
{
    public void HandleEKeyPress(ControlManager manager)
    {
        Debug.Log("ExploreState");
    }
}