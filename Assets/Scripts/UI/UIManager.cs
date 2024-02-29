using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIConfig uiConfig;

    private Dictionary<string, UIBase> uiScreens = new Dictionary<string, UIBase>();


    //µ¥ÀýÄ£Ê½
    private static UIManager instance;
   

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
        Debug.Log("UIManager Awake");
        instance = this;        
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterUI(string key, UIBase ui)
    {
        if (!uiScreens.ContainsKey(key))
        {
            uiScreens.Add(key, ui);
        }
    }

    public void OpenUI(string key)
    {
        if (uiScreens.TryGetValue(key, out UIBase ui))
        {
            ui.Open();
        }
    }

    public void CloseUI(string key)
    {
        if (uiScreens.TryGetValue(key, out UIBase ui))
        {
            ui.Close();
        }
    }
}
