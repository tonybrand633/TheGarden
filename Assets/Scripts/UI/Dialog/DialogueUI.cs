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
    public float typingSpeed = 0.02f; // 控制文字显示速度的参数

    private Queue<string> sentences;
    private string currentSentence;

    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        FindTheDialogueItems();
        sentences.Clear();
        //找到显示的组件        
        speakerNameText.text = dialogueData.speakerName;

        foreach (string sentence in dialogueData.sentences)
        {
            sentences.Enqueue(sentence);
        }
        //DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            UIManager.Instance.CloseUI("DialoguePanel");
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // 使用typingSpeed控制显示速度
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
