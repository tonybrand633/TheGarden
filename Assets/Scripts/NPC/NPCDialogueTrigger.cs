using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData; // �����NPC�ĶԻ�����
    public DialogueUI dialogueUI; // �Ի�������������
    public bool playerInRange = false; // ����Ƿ��ڷ�Χ�ڵı�־
    public GameObject pressE;

    void Start()
    {
        pressE.SetActive(false);         
    }

    void Update()
    {
        // �������ڷ�Χ�ڲ��Ұ�����E������ʼ�Ի�
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.OpenUI("DialoguePanel");                        
        }       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            
            playerInRange = true;

            pressE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressE.SetActive(false);
        }
    }
}
