using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	[Header("Vision")]
	public int visionRange;

    public const int maxHealth = 100;
    public int currentHealth = maxHealth;
    private HexCell cell;
	private HexGrid grid;

	private InfluenceMapSystem influence;

    void Start() {
		influence = GameObject.Find ("InfluenceMap").GetComponent<InfluenceMapSystem>();

        if (cell.Owner == 1)
        {
            this.tag = "edificioAliado";
			influence.UpdateBuildingInfluence (0);
            this.GetComponent<Influencer>()._Team = 1;
        }
        else if (cell.Owner == 2) {
            this.tag = "edificioEnemigo";
			influence.UpdateBuildingInfluence(1);
            this.GetComponent<Influencer>()._Team = 2;

        }
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();


    }

    // Update is called once per frame
    void Update()
    {
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();

        //  Debug.Log("la celda es ZELDA ---> " + cell);
        grid.IncreaseVisibility(cell, visionRange);

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
			grid.DecreaseVisibility (cell, visionRange);

			//Actualizar influencia quitarlo


            Destroy(this.gameObject, 0.5f);

        }
    }

    public void Celda(HexCell celda)
    {
        cell = celda;
    }

    public HexCell GetCelda()
    {
        return cell;
    }
}
