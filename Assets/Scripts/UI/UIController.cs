using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Button resetBtn;

    [SerializeField]
    GameObject Player;
    
    [SerializeField]
    Transform respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        resetBtn = transform.Find("ResetBtn").GetComponent<Button>();
        Player = GameObject.Find("Player");
        respawnPosition = GameObject.Find("RespawnPosition").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayerPosition() 
    {
        Player.transform.position = respawnPosition.transform.position;
    }    
}
