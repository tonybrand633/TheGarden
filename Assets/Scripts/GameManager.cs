using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    const string GameManagerKey = "GameManager";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); 
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitGameManager() 
    {
        Addressables.InstantiateAsync(GameManagerKey).Completed += OnInstantiated;
    }

    static void OnInstantiated(AsyncOperationHandle<GameObject> operationHandle) 
    {
        DontDestroyOnLoad(operationHandle.Result);
    }
}
