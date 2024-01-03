using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
