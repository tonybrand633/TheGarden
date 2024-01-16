using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavor : MonoBehaviour
{
    public bool canDialog;
    public bool dialogOpen;
    public bool dialogClose;

    public List<GameObject> npcList;
    public GameObject curNPC;

    //SomeSingal
    //Index0：对话框打开
    //Index1: 对话框结束
    //Index2:
    public bool[] SymbolRecive;

    PlayerMovement playerMovement;
    Rigidbody2D rig;


    void Start()
    {
        npcList = new List<GameObject>();
        playerMovement = GetComponent<PlayerMovement>();
        rig = GetComponent<Rigidbody2D>();
        SymbolRecive = new bool[2] { dialogOpen, dialogClose };
    }

    void Update()
    {
        GetTalkNpc();
        if (npcList.Count == 0)
        {
            canDialog = false;
        }
        else 
        {
            canDialog = true;
        }

        if (canDialog && Input.GetKeyDown(KeyCode.E)&&!dialogOpen)
        {
            GetTalkNpc();
            curNPC.GetComponent<NonPlayerCharacter>().OpenDialogUI();
        } else if (canDialog&&Input.GetKeyDown(KeyCode.E)&&dialogOpen)
        {
            curNPC.GetComponent<NonPlayerCharacter>().NextLine();
        }

        if (curNPC != null)
        {
            if (curNPC.GetComponent<NonPlayerCharacter>().canTalk)
            {
                ShowPressUI(curNPC);
            }
        }
        if (dialogOpen) 
        {
            CloseCurNPCUI();
        }



        //限制主角移动
        //if (dialogOpen)
        //{
        //    rig.velocity = Vector2.zero;
        //    rig.freezeRotation = true;
        //}
        //else
        //{
        //    rig.freezeRotation = false;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC")) 
        {
            GameObject cNpc = collision.transform.parent.gameObject;
            if (!npcList.Contains(cNpc)) 
            {
                npcList.Add(cNpc);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            GameObject curExitNpc = collision.transform.parent.gameObject;
            curExitNpc.GetComponent<NonPlayerCharacter>().HidePressEUI();
            if (npcList.Contains(curExitNpc)) 
            {
                npcList.Remove(curExitNpc);
            }                        
        }
    }

    void GetTalkNpc() 
    {
        if (npcList.Count >= 1)
        {
            GameObject tempNpc = npcList[0];
            float minDistance = float.MaxValue;
            foreach (var npc in npcList)
            {
                float curDistance = Vector2.Distance(this.transform.position, npc.transform.position);
                if (curDistance < minDistance)
                {
                    minDistance = curDistance;
                    tempNpc = npc;
                }
            }
            curNPC = tempNpc;
        }
        else 
        {
            curNPC = null;
        }
    }

    void CloseCurNPCUI()
    {
        curNPC.GetComponent<NonPlayerCharacter>().HidePressEUI();
    }

    void ShowPressUI(GameObject cNpc) 
    {
        cNpc.GetComponent<NonPlayerCharacter>().ShowPressEUI();
    }

    //接收UIManager发出的通知

    public void RecieveMessageFromUIManager(UIMessageSender result) 
    {
        for (int i = 0; i < result.UISignal.Length; i++)
        {
            SymbolRecive[i] = result.UISignal[i];
        }
        UpdateSymbol();
    }

    void UpdateSymbol() 
    {
        dialogOpen = SymbolRecive[0];
        dialogClose = SymbolRecive[1];
    }
}
