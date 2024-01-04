using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DialogUI : MonoBehaviour
{
    public string[] lines;
    public int index;

    public TextMeshProUGUI textComponent;

    public float textSpeed;

    void Awake()
    {
         
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialog(string[]text) 
    {
        lines = text;
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
