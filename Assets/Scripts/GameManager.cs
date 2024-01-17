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

    //在加载场景之前，启用这个方法
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitGameManager()
    {
        Addressables.InstantiateAsync(GameManagerKey).Completed += OnInstantiated;
        Debug.Log("<color=green>Init GameManager!!!!!!!!BY Async</color>");
    }

    static void OnInstantiated(AsyncOperationHandle<GameObject> operationHandle) 
    {
        //不要摧毁这个异步加载出来的东西
        DontDestroyOnLoad(operationHandle.Result);        
    }

    void Start()
    {
        SceneInfo sceneInfo = FindObjectOfType<SceneInfo>();
        if (sceneInfo!=null) 
        {
            hasUIScene = sceneInfo.hasUIScene;
        }

        //初始化UIManager
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
