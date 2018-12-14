using UnityEngine;
using UnityEngine.EventSystems;
//Añadido por marcos lo de generic
using System.Collections.Generic;

public class HexGameUI : MonoBehaviour {

	[Header("Añadidos por Marcos")]
	public GameObject paloma;
	public float alturaPaloma=50f;

	private GameObject newpaloma;
	private GameObject edificioOrigen;
	//UI original
	[Header("GameUI original")]

	public HexGrid grid;

	HexCell currentCell;

	HexUnit selectedUnit;

	public void SetEditMode (bool toggle) {
		enabled = !toggle;
		grid.ShowUI(!toggle);
		grid.ClearPath();
		if (toggle) {
			Shader.EnableKeyword("HEX_MAP_EDIT_MODE");
		}
		else {
			Shader.DisableKeyword("HEX_MAP_EDIT_MODE");
		}
	}




	void Update () {
		if (!EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButtonDown(0)) {
				DoSelection();
			}
			else if (selectedUnit) {
				if (Input.GetMouseButtonDown(1)) {//Marcos cambios para leer con la paloma
					//DoMove();

					CreatePidgeon(ClosestBuild(selectedUnit));


				}
				else {
					DoPathfinding();
				}
			}
		}
	}

	void DoSelection () {
		grid.ClearPath();
		UpdateCurrentCell();
		if (currentCell) {
			selectedUnit = currentCell.Unit;
		}
	}

	void DoPathfinding () {
		if (UpdateCurrentCell()) {
			if (currentCell && selectedUnit.IsValidDestination(currentCell)) {
				grid.FindPath(selectedUnit.Location, currentCell, selectedUnit);
			}
			else {
				grid.ClearPath();
			}
		}
	}

	void DoMove () {
		if (grid.HasPath) {
			selectedUnit.Travel(grid.GetPath());
			grid.ClearPath();
		}
	}

	bool UpdateCurrentCell () {
		HexCell cell =
			grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
		if (cell != currentCell) {
			currentCell = cell;
			return true;
		}
		return false;
	}
	private GameObject ClosestBuild (HexUnit unit){
		List<GameObject> edificios = new List<GameObject> (GameObject.FindGameObjectsWithTag ("edificioAliado"));//Tag de los edificios
		float distanciaUniEdi = 999999999999999999;

		foreach(GameObject edificio in edificios){
			float distanciaActual = distanciaUB (unit, edificio);
			if(distanciaUniEdi>distanciaActual){
				distanciaUniEdi = distanciaActual;
				edificioOrigen = edificio;
			}
		}
		return edificioOrigen;		
	}

	private float distanciaUB(HexUnit unidad,GameObject edificio){
		float realDistance = Mathf.Abs (Vector3.Distance (unidad.transform.position, edificio.transform.position));
		return realDistance;
	}

	private void CreatePidgeon (GameObject build) {//Crear palomo
		/*HexCell cell = GetCellUnderCursor();
		if (cell && !cell.Unit) {
			hexGrid.AddUnit(
				Instantiate(HexUnit.unitPrefab), cell, UnityEngine.Random.Range(0f, 360f)
			);
		}*/
		Vector3 origenPaloma = new Vector3 (build.transform.position.x, alturaPaloma, build.transform.position.z);
		newpaloma = Instantiate (paloma,origenPaloma,new Quaternion(0,0,0,0));//si se ve raro cambiar el quaternion
		List<HexCell>gridList=grid.GetPath();
		newpaloma.GetComponent<PalomaClass>().Objetivo(selectedUnit,gridList);
		/*selectedUnit.Travel(grid.GetPath());
		grid.ClearPath();*/
	}

}