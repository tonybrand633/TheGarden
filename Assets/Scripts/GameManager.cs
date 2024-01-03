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
        //��Ҫ�ݻ�����첽���س����Ķ���
        DontDestroyOnLoad(operationHandle.Result);        
    }

    void Start()
    {
        //��ʼ��UIManager
        uiManager = UIManager.Instance;    
    }
}
