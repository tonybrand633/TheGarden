using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : UIBase
{
    private bool isClosed;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    public string[] looplines;
    public string[] hiddenlines;

    public string[] targetLines;
    public float typingSpeed = 2f; // 控制文字显示速度的参数

    private int index;

    public void StartDialogue(DialogueData dialogueData)
    {        
        FindTheDialogueItems();
        dialogueText.text = string.Empty;
        index = 0;
        targetLines = dialogueData.sentences;
        looplines = dialogueData.loopsentences;
        hiddenlines = dialogueData.hiddensentences;
        //找到显示的组件     
        //之后的对话头像也是在这里设置   
        speakerNameText.text = dialogueData.speakerName;
        StartCoroutine(TypeLine());
    }

    public void KeyDownWay() 
    {
        if(isClosed)
        {
            targetLines = looplines;
        }
        if (dialogueText.text == targetLines[index])
        {
            DisplayNextLine();
        }
        else 
        {
            StopAllCoroutines();
            dialogueText.text = targetLines[index];
        }               
    }

    public void DisplayNextLine() 
    {
        if (index < targetLines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            isClosed = true;
            UIManager.Instance.CloseUI("DialoguePanel");
        }
    }

    public void DisplayLoopLine()
    {
        if(looplines.Length>1)
        {
            index = Random.Range(0,looplines.Length);                         
        }else
        {
            index = 0;
        }
        targetLines = looplines;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() 
    {
        foreach (char c in targetLines[index].ToCharArray()) 
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    

    public override void Open()
    {
        Debug.Log("开启UI");
        gameObject.SetActive(true);
        
    }

    public override void Close()
    {
        Debug.Log("Close UI Dialogue");
        gameObject.SetActive(false);
    }

    private void FindTheDialogueItems() 
    {
        Transform textPanel = transform.Find("TextPanel");
        speakerNameText = textPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        dialogueText = textPanel.Find("DialogueText").GetComponent<TextMeshProUGUI>(); 
    }
}
