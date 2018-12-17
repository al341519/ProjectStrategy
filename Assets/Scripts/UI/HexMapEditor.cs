using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System;
using System.Collections;

public class HexMapEditor : MonoBehaviour {

	public HexGrid hexGrid;

	public Material terrainMaterial;
	public HexUnit enemyUnitPrefab;

    //NUEVO
    public HexUnit[] unidad;

    public GameManager gameManager;

    Player player;

	int activeElevation;
	int activeWaterLevel;

    int activeUrbanLevel, activeFarmLevel, activePlantLevel, activeSpecialIndex;//, activeUnitIndex;

	int activeTerrainTypeIndex;

	int brushSize;

	bool applyElevation = true;
	bool applyWaterLevel = true;

    bool applyUrbanLevel, applyFarmLevel, applyPlantLevel, applySpecialIndex;//, applyUnitIndex;

	enum OptionalToggle {
		Ignore, Yes, No, Arround
	}

	OptionalToggle riverMode, roadMode, walledMode;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;

	public void SetTerrainTypeIndex (int index) {
		activeTerrainTypeIndex = index;
	}

	public void SetApplyElevation (bool toggle) {
		applyElevation = toggle;
	}

	public void SetElevation (float elevation) {
		activeElevation = (int)elevation;
	}

	public void SetApplyWaterLevel (bool toggle) {
		applyWaterLevel = toggle;
	}

	public void SetWaterLevel (float level) {
		activeWaterLevel = (int)level;
	}

	public void SetApplyUrbanLevel (bool toggle) {
		applyUrbanLevel = toggle;
	}

	public void SetUrbanLevel (float level) {
		activeUrbanLevel = (int)level;
	}

	public void SetApplyFarmLevel (bool toggle) {
		applyFarmLevel = toggle;
	}

	public void SetFarmLevel (float level) {
		activeFarmLevel = (int)level;
	}

	public void SetApplyPlantLevel (bool toggle) {
		applyPlantLevel = toggle;
	}

	public void SetPlantLevel (float level) {
		activePlantLevel = (int)level;
	}

	public void SetApplySpecialIndex (bool toggle) {
		applySpecialIndex = toggle;
	}

	public void SetSpecialIndex (float index) {
		activeSpecialIndex = (int)index;
	}


    public void SetBrushSize (float size) {
		brushSize = (int)size;
	}

	public void SetRiverMode (int mode) {
		riverMode = (OptionalToggle)mode;
	}

	public void SetRoadMode (int mode) {
		roadMode = (OptionalToggle)mode;
	}

	public void SetWalledMode (int mode) {
		walledMode = (OptionalToggle)mode;
	}

	public void SetEditMode (bool toggle) {
		enabled = toggle;
	}

	public void ShowGrid (bool visible) {
		if (visible) {
			terrainMaterial.EnableKeyword("GRID_ON");
		}
		else {
			terrainMaterial.DisableKeyword("GRID_ON");
		}
	}

	void Start () {
		terrainMaterial.DisableKeyword("GRID_ON");
		Shader.EnableKeyword("HEX_MAP_EDIT_MODE");
		SetEditMode(true);
        player = gameManager.getPlayer(1);
	}

	void Update () {
		if (!EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButton(0)) {
				HandleInput();
				return;
			}
			if (Input.GetKeyDown(KeyCode.U)) {
				if (Input.GetKey(KeyCode.LeftShift)) {
					DestroyUnit();
				}
				else {
					CreateUnit();
				}
				return;
			}
			if (Input.GetKeyDown(KeyCode.Y)) {
				if (Input.GetKey(KeyCode.LeftShift)) {
					DestroyUnit();
				}
				else {
					CreateUnitEmemy();
				}
				return;
			}
		}
		previousCell = null;
	}

	HexCell GetCellUnderCursor () {
		return
			hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
	}

	void CreateUnit () {
		HexCell cell = GetCellUnderCursor();
		if (cell && !cell.Unit) {
			hexGrid.AddUnit(
				Instantiate(HexUnit.unitPrefab), cell, UnityEngine.Random.Range(0f, 360f)
			);
		}
	}

    //NUEVO
    public void CreateUnidad(int id)
    {
        StartCoroutine(WaitForCell(id));
    }

    IEnumerator WaitForCell(int id)
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                HexCell cell = hexGrid.GetCell(ray);
                if (cell !=null)
                {
                    Debug.Log(cell.Position);
                    if (cell && !cell.Unit && cell.SpecialIndex ==0)
                    {
                        hexGrid.AddUnit(Instantiate(unidad[id]), cell, UnityEngine.Random.Range(0f, 360f));
                    }
                    yield break;
                }
            }
            yield return null;
        }
        //not here yield return null;
    }

    //Añadido por marcos para enemigos
    void CreateUnitEmemy () {
		HexCell cell = GetCellUnderCursor();
		if (cell && !cell.Unit) {
			hexGrid.AddUnit(
				Instantiate(enemyUnitPrefab), cell, UnityEngine.Random.Range(0f, 360f)
			);
		}
	}

	void DestroyUnit () {
		HexCell cell = GetCellUnderCursor();
		if (cell && cell.Unit) {
			hexGrid.RemoveUnit(cell.Unit);
		}
	}

	void HandleInput () {
		HexCell currentCell = GetCellUnderCursor();
		if (currentCell) {
			if (previousCell && previousCell != currentCell) {
				ValidateDrag(currentCell);
			}
			else {
				isDrag = false;
			}
			EditCells(currentCell);
			previousCell = currentCell;
		}
		else {
			previousCell = null;
		}
	}

	void ValidateDrag (HexCell currentCell) {
		for (
			dragDirection = HexDirection.NE;
			dragDirection <= HexDirection.NW;
			dragDirection++
		) {
			if (previousCell.GetNeighbor(dragDirection) == currentCell) {
				isDrag = true;
				return;
			}
		}
		isDrag = false;
	}

	void EditCells (HexCell center) {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++) {
			for (int x = centerX - r; x <= centerX + brushSize; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++) {
			for (int x = centerX - brushSize; x <= centerX + r; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}

	void EditCell (HexCell cell) {
		if (cell) {
			if (activeTerrainTypeIndex >= 0) {
				cell.TerrainTypeIndex = activeTerrainTypeIndex;
			}
			if (applyElevation) {
				cell.Elevation = activeElevation;
			}
			if (applyWaterLevel) {
				cell.WaterLevel = activeWaterLevel;
			}
			if (applySpecialIndex) {
                if (cell.SpecialIndex == 0)
                {
                    if (activeSpecialIndex == 1 && player.construir(activeSpecialIndex))
                    {
                        cell.Owner = 1;
                        cell.SpecialIndex = activeSpecialIndex;
                    }
                    else if (activeSpecialIndex <= 7 && cell.Walled && player.construir(activeSpecialIndex))
					{
                        cell.Owner = 1;
                        cell.SpecialIndex = activeSpecialIndex;
                    }
                    else if (activeSpecialIndex > 7 && !cell.Walled && player.construir(activeSpecialIndex))
                    {
                        cell.Owner = 1;
                        cell.SpecialIndex = activeSpecialIndex;
                    }
                    StartCoroutine(BuildBuilding(cell.SpecialIndex, cell));


                }
            }
			if (applyUrbanLevel) {
				cell.UrbanLevel = activeUrbanLevel;
			}
			if (applyFarmLevel) {
				cell.FarmLevel = activeFarmLevel;
			}
			if (applyPlantLevel) {
				cell.PlantLevel = activePlantLevel;
			}
			if (riverMode == OptionalToggle.No) {
				cell.RemoveRiver();
			}
			if (roadMode == OptionalToggle.No) {
				cell.RemoveRoads();
			}
            if (walledMode == OptionalToggle.Arround) {

                if (cell.SpecialIndex == 1)
                {
                    foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        cell.GetNeighbor(direction).Walled = true;
                        cell.Owner = 1;
                    }
                    cell.Walled = true;
                }
            }
			else if (walledMode != OptionalToggle.Ignore) {
				cell.Walled = walledMode == OptionalToggle.Yes;
			}
			if (isDrag) {
				HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
				if (otherCell) {
					if (riverMode == OptionalToggle.Yes) {
						otherCell.SetOutgoingRiver(dragDirection);
					}
					if (roadMode == OptionalToggle.Yes) {
						otherCell.AddRoad(dragDirection);
					}
				}
			}
		}
        resetValues();
	}

    public IEnumerator BuildBuilding(int index, HexCell cell) {
        Debug.Log("ENTRANDO CORRUTINA");
        yield return new WaitForSeconds(4);
   //     cell.SpecialIndex = ++index;
        applySpecialIndex = true;
        EditCell(cell);
        Debug.Log("index -> " + cell.SpecialIndex);
        Debug.Log("APLICADO EL CAMBIO");

    }

    void resetValues() {
        walledMode = OptionalToggle.Ignore;
        activeSpecialIndex = 0;
        applySpecialIndex = false;
    }

    


}