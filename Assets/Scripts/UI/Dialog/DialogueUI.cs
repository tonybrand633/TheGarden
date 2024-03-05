using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : UIBase
{
    public Text speakerNameText;
    public Text dialogueText;
    public float typingSpeed = 0.02f; // ����������ʾ�ٶȵĲ���

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
            yield return new WaitForSeconds(typingSpeed); // ʹ��typingSpeed������ʾ�ٶ�
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (dialogueText.text == currentSentence)
        //    {
        //        DisplayNextSentence();
        //    }
        //    else
        //    {
        //        StopAllCoroutines();
        //        dialogueText.text = currentSentence; // ������ʾ��������
        //    }
        //}
    }

    public override void Open()
    {
        Debug.Log("����UI");
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        Debug.Log("Close UI Dialogue");
        gameObject.SetActive(false);
    }
}
