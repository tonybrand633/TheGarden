using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

// ×´Ì¬½Ó¿Ú
public interface IState
{
    void HandleEKeyPress(ControlManager manager);
}

public class UIState : IState
{
    public void HandleEKeyPress(ControlManager manager)
    {
        Debug.Log("UI State");
    }
}

public class ExploreState : IState
{
    public void HandleEKeyPress(ControlManager manager)
    {
        Debug.Log("ExploreState");
    }
}