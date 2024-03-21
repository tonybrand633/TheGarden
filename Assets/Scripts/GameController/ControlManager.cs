using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    //这个是控制玩家的控制器
    private void Start()
    {
        //GameStateManager.Instance.SetState(new ExploreState());
    }

    void Update()
    {
        //在这里会直接传递我按下的E，来根据目前的State状态，来决定E的作用
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameStateManager.Instance.GetState().HandleEKeyPress(this);
        }
    }
}
