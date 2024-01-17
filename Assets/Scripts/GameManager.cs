using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    public string[] SceneName;

    const string GameManagerKey = "GameManager";

    public static GameManager instance;

    public static GameManager Instance 
    {
        get 
        {
            if (instance == null)
            {
                GameObject gameManagerObject = new GameObject("GameManager");
                instance = gameManagerObject.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    public bool hasUIScene = true;
    public UIManager uiManager;

    //�ڼ��س���֮ǰ�������������
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitGameManager()
    {
        Addressables.InstantiateAsync(GameManagerKey).Completed += OnInstantiated;
        Debug.Log("<color=green>Init GameManager!!!!!!!!BY Async</color>");
    }

    static void OnInstantiated(AsyncOperationHandle<GameObject> operationHandle) 
    {
        //��Ҫ�ݻ�����첽���س����Ķ���
        DontDestroyOnLoad(operationHandle.Result);        
    }

    void Start()
    {
        SceneInfo sceneInfo = FindObjectOfType<SceneInfo>();
        if (sceneInfo!=null) 
        {
            hasUIScene = sceneInfo.hasUIScene;
        }

        //��ʼ��UIManager
        if (hasUIScene) 
        {
            uiManager = UIManager.Instance;
        }

    }

    public void Test() 
    {
        Debug.Log("Callback From Player");
    }
}
