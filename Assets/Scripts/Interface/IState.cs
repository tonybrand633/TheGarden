using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

// ״̬�ӿ�
public interface IState
{
    void HandleEKeyPress(ControlManager manager);
}