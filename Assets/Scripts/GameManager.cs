using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string[] SceneName;

    const string GameManagerKey = "GameManager";



    private static GameManager instance;

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


    bool hasUIScene = true;
    public UIManager uiManager;
    public TimerManager timerManager;

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

        timerManager = TimerManager.Instance;

    }

    public void LoadSceneName(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int index) 
    {
        SceneManager.LoadScene(index);
    }


    public void AnalyzeTheSignal(SignalInfoHolder signalInfoHolder) 
    {
        string sceneName = signalInfoHolder.sceneName;
        int sceneIndex = signalInfoHolder.sceneIndex;
        if (sceneName != "")
        {
            LoadSceneName(sceneName);
        } else if (sceneIndex!=-1) 
        {
            LoadSceneByIndex(sceneIndex);
        }
    }
}
