using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    void Update()
    {
        //�������ֱ�Ӵ����Ұ��µ�E��������Ŀǰ��State״̬��������E������
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameStateManager.Instance.GetState().HandleEKeyPress(this);
        }
    }
}
