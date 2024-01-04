using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //单例模式
    private static UIManager instance;


    [Header("UI组件")]
    public Canvas canvas;
    public DialogUI dialogUI;


    [Header("通知对象")]
    public PlayerBehavor playerBehavor;

    public static UIManager Instance
    {
        get
        {
            if (instance == null) 
            {
                GameObject uiManagerObject = new GameObject("UIManager");
                instance = uiManagerObject.AddComponent<UIManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        dialogUI = canvas.transform.Find("DialogBox").GetComponent<DialogUI>();
        playerBehavor = FindObjectOfType<PlayerBehavor>();
    }

    /// <summary>
    ///  UIDialog Methods
    /// </summary>
    public void OpenDialog(string[]textContent) 
    {
        GameObject dialogBox = dialogUI.gameObject;
        dialogBox.SetActive(true);
        Transform dT = dialogBox.transform;
        for (int i = 0; i < dT.childCount; i++)
        {
            Transform t = dT.GetChild(i).transform;
            t.gameObject.SetActive(true);
        }
        
        SendMessageToPlayer(0);
        dialogUI.StartDialog(textContent);
    }

    public void NextDialogLine() 
    {
        dialogUI.NextDialog();
    }

    public void SendMessageToPlayer(int index) 
    {
        UIMessageSender sender = new UIMessageSender(index,playerBehavor.SymbolRecive);
        playerBehavor.RecieveMessageFromUIManager(sender);
    }
    
}
