using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DialogUI : MonoBehaviour
{
    [Header("显示文本选项")]
    public string[] lines;
    public int index;
    public float textSpeed;

    TextMeshProUGUI textComponent;

    public void StartDialog(string[]text) 
    {
        lines = text;
        textComponent = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();
        if (textComponent!=null) 
        {
            ShowChar();
        }
    }

    public void NextDialog() 
    {
        if (textComponent.text == lines[index])
        {
            index++;
            NextLine();
        }
        else 
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }

    public void ShowChar() 
    {
        StartCoroutine(ShowTextByTime());
    }

    IEnumerator ShowTextByTime() 
    {
        foreach (char c in lines[index].ToCharArray())
         {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() 
    {
        if (index < lines.Length)
        {
            textComponent.text = string.Empty;
            StartCoroutine(ShowTextByTime());
        }
        else 
        {
            gameObject.SetActive(false);
            index = 0;
            textComponent.text = string.Empty;
            UIManager.Instance.SendMessageToPlayer(1);
        }
    }
}
