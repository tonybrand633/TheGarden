using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

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

            Vector3 moveToPos = targetPosition;

            moveToPos = new Vector3(targetPosition.x, targetPosition.y, -10);

            transform.position = moveToPos;
        }
    }
}
