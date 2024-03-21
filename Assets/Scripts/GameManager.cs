using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("����λ��")]
    private GameObject Player;
    private int spawnID;
    private int faceRight;

    [Header("������Ϣ")]
    public bool hasUIScene;

    public bool hasPlayer;

    const string GameManagerKey = "GameManager";


    public SceneInfo curSceneInfo;


    [Header("��Ϸ��Manager")]    
    public UIManager uiManager;
    public GameStateManager stateManager;
    public TimerManager timerManager;

    private static GameManager instance;

    public static GameManager Instance 
    {
        get 
        {
            if (instance == null)
            {
                Debug.Log("Instance is NULL");     
            }
            return instance;
        }
    }

    //�ڼ��س���֮ǰ�������������
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitGameManager()
    {        
        if(instance==null)
        {
            Addressables.InstantiateAsync(GameManagerKey).Completed += OnInstantiated;            
            Debug.Log("<color=green>Init GameManager!!!!!!!!BY Async</color>");
        }
    }

    static void OnInstantiated(AsyncOperationHandle<GameObject> operationHandle) 
    {
        instance = operationHandle.Result.GetComponent<GameManager>();
        //��Ҫ�ݻ�����첽���س����Ķ���
        DontDestroyOnLoad(operationHandle.Result);        
    }

    void Start()
    {
        if(GameObject.Find("Player")!=null)
        {
            Player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
            Player.GetComponent<PlayerMovement>().isFacingRight = true;
            Camera main = Camera.main;
            main.AddComponent<CameraFollow>();
        }
        if (!PlayerPrefs.HasKey("FirstLoadScene")) 
        {
            Debug.Log("Start Load Scene");
            GetSceneInfo();
            InitAllManager();
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

    // ���л������󣬷�����һ������������
    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        GetSceneInfo();        
        if(hasPlayer)
        {
            //Ѱ����ҽ�ɫ
            Player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
        }
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

    
    //UIManager��һЩ��������Manager���������
    private void GetSceneInfo() 
    {
        Debug.Log("<color=green>GetSceneInfo!!!!!!</color>");
        curSceneInfo = FindObjectOfType<SceneInfo>();
        //curSceneInfo = GameObject.Find("SceneInfo").GetComponent<SceneInfo>();
        hasUIScene = curSceneInfo.hasUIScene;
        hasPlayer = curSceneInfo.hasPlayer;
        //��ʼ��UIManager,���Ҹ��ݳ�������Ϣ�����ض�Ӧ��UI
        if (hasUIScene)
        {
            uiManager = UIManager.Instance;
            uiManager.InitializedUIConfig(curSceneInfo.uiConfig);
        }
        //Ҳ���Գ�ʼ��������Manager
    }

    private void InitAllManager() 
    {        
        stateManager = GameStateManager.Instance;        
    }


    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        Debug.Log("<color=yellow>Enable</color>");
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        Debug.Log("<color=yellow>Disable</color>");
    }

    //�ر�Ӧ�ú�����������
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
