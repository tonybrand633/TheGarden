using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour,ICanTalkWith
{
    //对话文本的分类设置以及状态
    public bool canTalk;
    bool canLoopTalk;
    bool isFirstTalk;
    bool dialogTrigger;

    [Header("对话文本设置")]
    public DialogText dialogText;
    
    public string[] openContent;
    public string[] loopContent;
    public string[] hiddenContent;

    [Header("解锁通知对象")]
    public NonPlayerCharacter[] senderRecieveObject;

    [Header("对话UI显示")]
    public GameObject pressE;  //需要随着NPC一起转向的物体


    // Start is called before the first frame update
    void Start()
    {
        canTalk = true;
        isFirstTalk = true;

        if (pressE != null)
        {
            pressE.SetActive(false);
        }

        if (dialogText.openText.Length!=0)
        {
            openContent = dialogText.openText;
        }
        else 
        {
            canTalk = false;
        }
        if (dialogText.loopText.Length!=0) 
        {
            loopContent = dialogText.loopText;
            canLoopTalk = true;
        }
        if (dialogText.hiddenText!.Length!=0) 
        {
            hiddenContent = dialogText.hiddenText;
        }
    }

    public void OpenDialogUI()
    {
        //若不能对话，则返回
        if (!canTalk) 
        {
            return;
        }
        //如果有秘密对话，则触发秘密/剧情对话
        if (dialogTrigger) 
        {
            dialogTrigger = false;
            UIManager.Instance.OpenDialog(hiddenContent);
        }
        //如果是初次对话，则触发初次对话文本
        else if(isFirstTalk&&!dialogTrigger)
        {
            if (senderRecieveObject.Length!=0) 
            {
                for (int i = 0; i < senderRecieveObject.Length; i++)
                {
                    NonPlayerCharacter recieveObj = senderRecieveObject[i];
                    recieveObj.dialogTrigger = true;
                }
            }
            UIManager.Instance.OpenDialog(openContent);
            isFirstTalk = false;
            if (!canLoopTalk) 
            {
                canTalk = false;
            }
        }
        //如果有循环对话，则一直触发循环对话
        else if(canLoopTalk)
        {
            int index = Random.Range(0, loopContent.Length);
            string res = loopContent[index];
            string[]result = new string[1] { res};
            UIManager.Instance.OpenDialog(result);
        }
    }

    public void NextLine() 
    {
        UIManager.Instance.NextDialogLine();
    }

    public void ShowPressEUI() 
    {
        pressE.SetActive(true);
        pressE.GetComponent<Animator>().Play("PressEAnimation");
    }

    public void HidePressEUI() 
    {
        pressE.SetActive(false);
    }
}
