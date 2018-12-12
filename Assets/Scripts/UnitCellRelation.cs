using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCellRelation : MonoBehaviour {

	GameObject[] unitArr;
	CellClass controlCell;
	HexCell cell;

	// Use this for initialization
	void Awake () {
		unitArr = GameObject.FindGameObjectsWithTag ("AllyUnit");
		controlCell = this.GetComponent<CellClass> ();
		cell = this.GetComponent<HexCell> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (cell.Unit) {
			controlCell.ocupada = true;	
		} else {
			controlCell.ocupada = false;
		}
		/*unitArr = GameObject.FindGameObjectsWithTag ("Unity");
		for (int i = 0; i < (unitArr.Length - 1); i++) {
			if (unitArr [i].transform.position == this.transform.position) {
				Debug.Log ("Estoy en el if");
				controlCell.ocupada = true;
				break;
			} 
			else {
				Debug.Log ("Estoy en el else");
				controlCell.ocupada = false;
			}
		}*/
	}
}

