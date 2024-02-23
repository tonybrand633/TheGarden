using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //单例模式
    private static UIManager instance;


    [Header("UI组件")]
    public Canvas canvas;

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
}
