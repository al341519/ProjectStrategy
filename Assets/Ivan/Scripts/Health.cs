﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public const int maxHealth = 100;
    public int currentHealth = maxHealth;
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
            //UPDATE EN EL MAPA INFLUENCIA
            Destroy(this, 0.5f);
        }
    }
}
