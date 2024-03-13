using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData; // �����NPC�ĶԻ�����
    public DialogueUI dialogueUI; // �Ի�������������
    public bool playerInRange = false; // ����Ƿ��ڷ�Χ�ڵı�־


    private bool dialogueOpen;
    private bool dialogueContinue;

    public GameObject pressE;

    void Start()
    {
        pressE.SetActive(false);         
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {            
            playerInRange = true;
            pressE.SetActive(true);
            GameStateManager.Instance.SetState(new UIState());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressE.SetActive(false);
            GameStateManager.Instance.SetState(new ExploreState());
        }
    }
}
