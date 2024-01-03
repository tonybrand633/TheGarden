using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    const string GameManagerKey = "GameManager";

    public UIManager uiManager;

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
        //初始化UIManager
        uiManager = UIManager.Instance;    
    }
}
