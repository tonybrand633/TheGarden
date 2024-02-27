using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : UIBase
{
    public Text speakerNameText;
    public Text dialogueText;
    public float typingSpeed = 0.02f; // 控制文字显示速度的参数

    private Queue<string> sentences;
    private string currentSentence;

    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        Open();
        sentences.Clear();
        speakerNameText.text = dialogueData.speakerName;

        foreach (string sentence in dialogueData.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            Close();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueText.text == currentSentence)
            {
                DisplayNextSentence();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence; // 立即显示完整句子
            }
        }
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
