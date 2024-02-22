using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject Player;
    private int spawnID;
    private int faceRight;
    private bool hasUIScene = true;

    const string GameManagerKey = "GameManager";


    public SceneInfo curSceneInfo;
    public UIManager uiManager;
    public TimerManager timerManager;

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
        if (!PlayerPrefs.HasKey("FirstLoadScene")) 
        {
            Player = GameObject.FindObjectOfType<PlayerBehavor>().gameObject;
            Player.GetComponent<PlayerMovement>().isFacingRight = true;
            Debug.Log("Start Load Scene");
            GetSceneInfo();
            PlayerPrefs.SetInt("FirstLoadScene", 1);
        }        
    }

    public void LoadSceneName(string sceneName) 
    {        
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int index) 
    {
        SceneManager.LoadScene(index);
    }

    // 在切换场景后，返回上一个场景的坐标
    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        GetSceneInfo();
        //寻找玩家角色
        Player = GameObject.FindObjectOfType<PlayerBehavor>().gameObject;
        if (PlayerPrefs.HasKey("SpawnID")&&PlayerPrefs.HasKey("SpawnFaceRight")) 
        {
            spawnID = PlayerPrefs.GetInt("SpawnID");
            faceRight = PlayerPrefs.GetInt("SpawnFaceRight");
        }
        Vector3 spawnPos = curSceneInfo.spawnPos[spawnID].position;
        if (Player != null)
        {
            //Debug.Log("SceneChanged And Find Player!!!"+"<color=green>Change Scene </color>"+nextScene.name+" from "+nextScene.name);      
            Player.transform.position = spawnPos;
            if (faceRight == -1)
            {
                Debug.Log("Face Left");
                Vector3 localScale = Player.transform.localScale;
                localScale.x *= -1;
                Player.transform.localScale = localScale;
                Player.GetComponent<PlayerMovement>().isFacingRight = false;
            }
            else 
            {
                Debug.Log("Face Right");
                Player.GetComponent<PlayerMovement>().isFacingRight = true;
            }
            
        }      
    }

    

    private void GetSceneInfo() 
    {
        Debug.Log("<color=green>New Scene!!!GetSceneInfo!!!!!!</color>");
        curSceneInfo = FindObjectOfType<SceneInfo>();
        hasUIScene = curSceneInfo.hasUIScene;

        //初始化UIManager
        if (hasUIScene)
        {
            uiManager = UIManager.Instance;
            uiManager.InitUIManager();
        }
        //初始化timerManager
        timerManager = TimerManager.Instance;
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        //Debug.Log("<color=yellow>Enable</color>");
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        //Debug.Log("<color=yellow>Disable</color>");
    }

    //关闭应用后采用这个方法
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("FirstLoadScene");
    }

    public void AnalyzeTheSignal(SignalInfoHolder signalInfoHolder)
    {               
        spawnID = signalInfoHolder.spawnID;
        PlayerPrefs.SetInt("SpawnID", spawnID);
        if (signalInfoHolder.isFaceRight)
        {
            faceRight = 1;
        }
        else 
        {
            faceRight = -1;
        }
        PlayerPrefs.SetInt("SpawnFaceRight", faceRight);
        string sceneName = signalInfoHolder.sceneName;
        int sceneIndex = signalInfoHolder.sceneIndex;
        if (sceneName != "")
        {
            LoadSceneName(sceneName);
        }
        else if (sceneIndex != -1)
        {
            LoadSceneByIndex(sceneIndex);
        }
    }
}
