using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;
    public static GameStateManager Instance 
    {
        get
        {
            if (instance == null) 
            {
                GameObject GameStateManagerObj = new GameObject("StateManager");
                instance = GameStateManagerObj.AddComponent<GameStateManager>();
            }
            return instance;
        }    
    }

    private IState currentState;

    void Awake()
    {
        Debug.Log("GameStateManager::Awake");
        SetState(new ExploreState());
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(IState newState)
    {
        currentState = newState;
    }

    public IState GetState()
    {
        return currentState;
    }
}


