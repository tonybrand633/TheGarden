using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currenVelocity = rig.velocity;
        Debug.DrawLine(transform.position,(Vector2)transform.position+currenVelocity,Color.red,0.1f);
    }
}
