using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityClass : MonoBehaviour {
    public int velocity;
    public float defence;
    public float attack;
    public int life;
    public int visionRange;
    public int attackRange;
    public Mode mode;

	// Use this for initialization
	void Start () {
        velocity = 0;
        defence = 0f;
        attack = 0f;
        life = 0;
        visionRange = 0;
        attackRange = 0;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
