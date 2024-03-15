using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : UIBase
{
    private DialogueData curData;
    private bool isClosed;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    public Image HeadIcon;
    public Sprite icon;

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

        curData = dialogueData;
        targetLines = dialogueData.sentences;
        looplines = dialogueData.loopsentences;
        hiddenlines = dialogueData.hiddensentences;
        icon = dialogueData.headicon;

        if(curData.isClosed)
        {
            targetLines = looplines;
        }
        //找到显示的组件     
        //之后的对话头像也是在这里设置   
        HeadIcon.sprite = icon;
        //设置头像居中显示
        //HeadIcon.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        //HeadIcon.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        //HeadIcon.rectTransform.anchoredPosition = new Vector2(0, 0);
        
        
        speakerNameText.text = dialogueData.speakerName;
        StartCoroutine(TypeLine());
    }

    public void KeyDownWay() 
    {
        if(curData.isClosed)
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
            curData.isClosed = true;
            curData = null;
            UIManager.Instance.CloseUI("DialoguePanel");
        }
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
        Transform headPanel = transform.Find("Headicon");
        speakerNameText = textPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        dialogueText = textPanel.Find("DialogueText").GetComponent<TextMeshProUGUI>(); 
        HeadIcon = headPanel.GetComponent<Image>();
    }
}
