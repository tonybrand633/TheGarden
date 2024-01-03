using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDetect")) 
        {
            Debug.Log("Enter");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDetect"))
        {
            Debug.Log("Exit");
        }
    }
}
