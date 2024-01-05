using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour,ICanTalkWith
{
    //对话文本的分类设置以及状态
    bool canTalk;
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


    // Start is called before the first frame update
    void Start()
    {
        canTalk = true;
        isFirstTalk = true;

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
        if (!canTalk) 
        {
            return;
        }
        if (dialogTrigger) 
        {
            dialogTrigger = false;
            UIManager.Instance.OpenDialog(hiddenContent);
        }else if(isFirstTalk&&!dialogTrigger)
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
        }
        else if(canLoopTalk)
        {
            int index = Random.Range(0, loopContent.Length);
            string res = loopContent[index];
            Debug.Log(index);
            string[]result = new string[1] { res};
            UIManager.Instance.OpenDialog(result);
        }
    }

    public void NextLine() 
    {
        UIManager.Instance.NextDialogLine();
    }
}
