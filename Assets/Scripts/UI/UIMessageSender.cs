using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageSender
{
    //SomeSingal
    //Index0：对话框打开
    //Index1: 对话框结束
    //Index2:
    public bool[] UISignal;

    public UIMessageSender(int index, bool[]symbolList) 
    {
        UISignal = new bool[symbolList.Length];
        for (int i = 0; i < UISignal.Length; i++)
        {
            UISignal[i] = false;
        }
        UISignal[index] = true;        
    }
}
