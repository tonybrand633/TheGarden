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
        textComponent = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        ShowChar();
    }

    // Update is called once per frame
    void Update()
    {

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
}
