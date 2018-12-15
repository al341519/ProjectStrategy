using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public const int maxHealth = 100;
    public int currentHealth = maxHealth;
    private HexCell cell;

    void Start() {
        if (cell.Owner == 1)
        {
            this.tag = "edificioAliado";
        }
        else if (cell.Owner == 2) {
            this.tag = "edificioEnemigo";
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
		Debug.Log (": "+currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
            //UPDATE EN EL MAPA INFLUENCIA
            cell.SpecialIndex = 0;
            cell.Owner = 0;
            cell.edificio = null;


            Destroy(this.gameObject, 0.5f);

        }
    }

    public void Celda(HexCell celda)
    {
        cell = celda;
    }
}
