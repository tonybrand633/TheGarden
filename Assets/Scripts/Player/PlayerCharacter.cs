using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public NewPlayerData playeData;
    private PlayerMovementBase playerMovementBase;

    public float Speed;
    public bool isFaceRight = true;
    

    void Awake()
    {
        playerMovementBase = gameObject.GetComponent<PlayerMovementBase>();
        AnalyzePlayerData();
    }
    

    // Update is called once per frame
    void Update()
    {
        float MoveX = 0;
        float MoveY = 0;
        if(Input.GetKey(KeyCode.W))
        {
            MoveY = 1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            MoveY = -1f;
        } 
        if(Input.GetKey(KeyCode.D))
        {
            MoveX = 1f;
        } 
        if(Input.GetKey(KeyCode.A))
        {
            MoveX = -1f;
        }    
        Vector3 movedir = new Vector3(MoveX,MoveY).normalized;
        transform.position += movedir*Speed*Time.deltaTime;
    }

    void AnalyzePlayerData()
    {
        Speed = playeData.Speed;
    }
}
