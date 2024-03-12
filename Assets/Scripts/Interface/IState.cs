using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

// ×´Ì¬½Ó¿Ú
public interface IState
{
    void HandleEKeyPress(ControlManager manager);
}