using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherClass : UnityClass {

	// Use this for initialization
	void Start () {
        velocity = 2;
        defence = 0.5f;
        attack = 1.5f;
        life = 1;
        visionRange = 3;
        attackRange = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
