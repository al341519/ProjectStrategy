using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    Rigidbody rb;
    float velocityY;
    float velocityX;
    float velocityZ;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update () {
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
        /*velocityY = rb.velocity.y;
        velocityX = rb.velocity.x;
        velocityZ = rb.velocity.z;
        float combVelocity = Mathf.Sqrt(velocityX * velocityX + velocityZ * velocityZ);
        float fallAngle = -1 * Mathf.Atan2(velocityY, combVelocity) * 180 / Mathf.PI;
        transform.eulerAngles = new Vector3(fallAngle, 
            transform.eulerAngles.y, transform.eulerAngles.z);*/
    }
}
