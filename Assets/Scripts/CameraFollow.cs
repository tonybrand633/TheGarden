using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float camSize;
    Vector3 moveToPos;

    public Transform LeftBound;
    public Transform RightBound;
    
    public Transform target;

    void Start()
    {
        camSize = Camera.main.orthographicSize;        
    }

    void Update()
    {
        if(GameManager.Instance.hasPlayer)
        {
            Debug.Log("Find Player");
            target = GameObject.Find("Player").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target!=null) 
        {            
            Vector3 targetPosition = target.transform.position;

            //Vector3 moveToPos = Vector3.Lerp(transform.position, targetPosition, u*Time.deltaTime);
            if (LeftBound && RightBound != null)
            {
                moveToPos.x = Mathf.Clamp(targetPosition.x, LeftBound.transform.position.x + camSize * 2, RightBound.transform.position.x - camSize * 2);
            }
            else 
            {
                moveToPos.x = targetPosition.x;
            }            
            moveToPos.z = transform.position.z;

            transform.position = moveToPos;
        }
    }
}
