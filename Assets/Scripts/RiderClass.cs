using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderClass : UnityClass {

	// Use this for initialization
	void Start () {
        velocity = 3;
        defence = 1f;
        attack = 1f;
        life = 1;
        visionRange = 3;
        attackRange = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
