using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerClass : UnityClass {

	// Use this for initialization
	void Start () {
        velocity = 1;
        defence = 1f;
        attack = 0f;
        life = 1;
        visionRange = 2;
        attackRange = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
