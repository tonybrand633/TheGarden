using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavor : MonoBehaviour
{
    public bool canDialog;
    public bool dialogOpen;
    public bool dialogClose;

    public NonPlayerCharacter curNPC;

    public bool[] SymbolRecive;

    PlayerMovement playerMovement;
    Rigidbody2D rig;

    void Awake()
    {
        
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rig = GetComponent<Rigidbody2D>();
        SymbolRecive = new bool[2] { dialogOpen, dialogClose };
    }

    void Update()
    {
        if (canDialog && Input.GetKeyDown(KeyCode.E)&&!dialogOpen)
        {
            curNPC.OpenDialogUI();
        } else if (canDialog&&Input.GetKeyDown(KeyCode.E)&&dialogOpen)
        {
            curNPC.NextLine();
        }

        if (dialogOpen)
        {
            rig.velocity = Vector2.zero;
            rig.isKinematic = true;
            playerMovement.enabled = false;
        }
        else 
        {
            rig.isKinematic = false;
            playerMovement.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDetect")) 
        {
            canDialog = true;
            curNPC = collision.gameObject.GetComponentInParent<NonPlayerCharacter>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDetect"))
        {
            canDialog = false;
            curNPC = null;
        }
    }

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
