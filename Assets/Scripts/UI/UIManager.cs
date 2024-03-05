using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UIConfig uiConfig;

    private Dictionary<string, UIBase> uiScreens = new Dictionary<string, UIBase>();


    //单例模式
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

    public void InitializedUIConfig(UIConfig Config) 
    {

        RemoveUI();

        uiConfig = Config;
        Canvas canvas = CreateCanvas();
        Transform UIParent = canvas.transform;

        foreach (UIItem uiItem in uiConfig.uiItems)
        {
            UIBase baseUIScripts = uiItem.uiPrefab;

            UIBase UIPanel = Instantiate(baseUIScripts, UIParent);
            RegisterUI(uiItem.key, UIPanel);
            //CloseUI(uiItem.key);
        }
    }

    private Canvas CreateCanvas()
    {
        // 创建Canvas GameObject
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // 确保Canvas是UI的根
        canvasObject.layer = LayerMask.NameToLayer("UI");

        return canvas;
    }

    public void RegisterUI(string key, UIBase ui)
    {
        if (!uiScreens.ContainsKey(key))
        {
            uiScreens.Add(key, ui);
        }
    }

    public void RemoveUI() 
    {
        uiScreens.Clear();
    }

    public void OpenUI(string key)
    {

        if (uiScreens.TryGetValue(key, out UIBase ui))
        {
            Debug.Log("OpenUI:" + key);
            ui.gameObject.SetActive(true);

        }
    }

    public void CloseUI(string key)
    {        
        if (uiScreens.TryGetValue(key, out UIBase ui))
        {
            Debug.Log("Find N Close:" + key);
            ui.gameObject.SetActive(false);
        }
    }
}
