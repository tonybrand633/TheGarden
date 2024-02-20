using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeerCanState 
{
    Empty,
    Saccharification,
    Boiled,
    Fermentation,
    Done
}

public abstract class BeerCanBase : MonoBehaviour
{
    public BeerCanState currentState;
    public float timer;
    protected TimerManager timerManager;

    protected virtual void Awake()
    {
        timerManager = TimerManager.Instance;
    }

    public abstract void StartBrewingProcess();
    public abstract void NextState();
}

