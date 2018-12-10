using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Influencer : MonoBehaviour {

    public int influenceRadius = 10;
    public float influencePower = 1;
    public float influencePropagationRatio = 0.75f;
    public bool isLineal = false;
    public Color color;
    public int player = 1;


    void Start () {
        tag = "Influencer";
	}
	
	void Update () {
		
	}
}
