using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClass : MonoBehaviour {
	public int mobility;
	public int vision;
	public int defense;
	public int range;

	private HexCell thisCell;
	private int typeIndex;

	// Use this for initialization
	void Awake () {
		mobility = 0;
		vision = 0;
		defense = 0;
		range = 0;

		thisCell = this.GetComponent<HexCell> ();
		typeIndex = thisCell.TerrainTypeIndex;
	}
	
	// Update is called once per frame
	void Update () {
		typeIndex = thisCell.TerrainTypeIndex;
		TerrainType ();

	}

	private void TerrainType(){
		switch (typeIndex) {
		case 0://Arena/Desierto
			mobility=50;//-1
			range=0;
			defense=0;
			vision=0;
			break;
		case 1://Hierba
			mobility=1;
			range=0;
			defense=0;
			vision=0;
			break;
		case 2://Barro no hay datos por lo que lo considero terreno dificil similar a la piedra sin aumento de vision y defensa
			mobility=50;//-1
			range=2;
			defense=0;
			vision=0;
			break;
		case 3://Piedra
			mobility=50;//-1
			range=2;
			defense=2;
			vision=2;
			break;
		case 4://Nieve
			mobility=50;//-1
			range=0;
			defense=0;
			vision=0;
			break;
		default:
			break;
		}
	}
}
