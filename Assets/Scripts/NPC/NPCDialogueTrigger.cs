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
        dialogueUI = FindObjectOfType<DialogueUI>(); // ���ҳ����еĶԻ�������ʵ��
    }

    void Update()
    {
        // �������ڷ�Χ�ڲ��Ұ�����E������ʼ�Ի�
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.OpenUI("DialoguePanel");
            dialogueUI.StartDialogue(dialogueData);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.OpenUI("DialoguePanel");
            
            dialogueUI = FindObjectOfType<DialogueUI>(); // ���ҳ����еĶԻ�������ʵ��
            dialogueUI.StartDialogue(dialogueData);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PlayerEnter");
        if (other.CompareTag("Player")) 
        {
            
            playerInRange = true;

            pressE.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressE.SetActive(false);
        }
    }
}
