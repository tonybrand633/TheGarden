using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour,ICanTalkWith
{
    //�Ի��ı��ķ��������Լ�״̬
    public bool canTalk;
    bool canLoopTalk;
    bool isFirstTalk;
    bool dialogTrigger;

    [Header("�Ի��ı�����")]
    public DialogText dialogText;
    
    public string[] openContent;
    public string[] loopContent;
    public string[] hiddenContent;

    [Header("����֪ͨ����")]
    public NonPlayerCharacter[] senderRecieveObject;

    [Header("�Ի�UI��ʾ")]
    public GameObject pressE;  //��Ҫ����NPCһ��ת�������


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
        //�����ܶԻ����򷵻�
        if (!canTalk) 
        {
            return;
        }
        //��������ܶԻ����򴥷�����/����Ի�
        if (dialogTrigger) 
        {
            dialogTrigger = false;
            UIManager.Instance.OpenDialog(hiddenContent);
        }
        //����ǳ��ζԻ����򴥷����ζԻ��ı�
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
        //�����ѭ���Ի�����һֱ����ѭ���Ի�
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
