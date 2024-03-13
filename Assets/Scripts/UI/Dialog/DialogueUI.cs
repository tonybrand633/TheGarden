using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : UIBase
{
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    public string[] lines;
    public float typingSpeed = 2f; // 控制文字显示速度的参数

    private int index;

    public void StartDialogue(DialogueData dialogueData)
    {
        FindTheDialogueItems();
        dialogueText.text = string.Empty;
        index = 0;
        lines = dialogueData.sentences;
        //找到显示的组件        
        speakerNameText.text = dialogueData.speakerName;
        StartCoroutine(TypeLine());
    }

    public void KeyDownWay() 
    {
        if (dialogueText.text == lines[index])
        {
            DisplayNextLine();
        }
        else 
        {
            StopAllCoroutines();
            dialogueText.text = lines[index];
        }               
    }

    public void DisplayNextLine() 
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            UIManager.Instance.CloseUI("DialoguePanel");
        }
    }

    IEnumerator TypeLine() 
    {
        foreach (char c in lines[index].ToCharArray()) 
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
