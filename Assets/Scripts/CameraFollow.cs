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
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target!=null) 
        {
            Vector3 targetPosition = target.transform.position;

            //Vector3 moveToPos = Vector3.Lerp(transform.position, targetPosition, u*Time.deltaTime);           
            moveToPos.x = Mathf.Clamp(targetPosition.x,LeftBound.transform.position.x+camSize*2,RightBound.transform.position.x-camSize*2);
            moveToPos.z = transform.position.z;

            transform.position = moveToPos;
        }
    }
}
