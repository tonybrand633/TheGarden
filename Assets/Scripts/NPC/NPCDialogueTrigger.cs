using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData; // 分配给NPC的对话数据
    public DialogueUI dialogueUI; // 对话管理器的引用
    public bool playerInRange = false; // 玩家是否在范围内的标志
    public GameObject pressE;

    void Start()
    {
        pressE.SetActive(false);
        dialogueUI = FindObjectOfType<DialogueUI>(); // 查找场景中的对话管理器实例
    }

    void Update()
    {
        // 如果玩家在范围内并且按下了E键，则开始对话
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.OpenUI("DialoguePanel");
            dialogueUI.StartDialogue(dialogueData);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.OpenUI("DialoguePanel");
            
            dialogueUI = FindObjectOfType<DialogueUI>(); // 查找场景中的对话管理器实例
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
