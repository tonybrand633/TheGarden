using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    //����ǿ�����ҵĿ�����
    private void Start()
    {
        //GameStateManager.Instance.SetState(new ExploreState());
    }

    void Update()
    {
        //�������ֱ�Ӵ����Ұ��µ�E��������Ŀǰ��State״̬��������E������
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameStateManager.Instance.GetState().HandleEKeyPress(this);
        }
    }
}
