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
                DontDestroyOnLoad(uiManagerObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
