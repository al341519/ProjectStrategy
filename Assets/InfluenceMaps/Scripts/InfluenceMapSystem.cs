using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMapSystem : MonoBehaviour {

    Influencer[] influencers;
    public GameObject[] mapPlanes;

    public List<Influencer>[] Units { get; set; }
    public List<Influencer>[] Buildings { get; set; }
    public List<Influencer> MapResources { get; set; }

    InfluenceMap[] militaryMaps;
    InfluenceMap[] economyMaps;
    HexGrid grid;

    public Vector2Int _Size = new Vector2Int(100, 66);

    public int _NumberOfPlayers = 2;
    public int _MapHeight = 100;

    bool hasMilitaryCoroutineEnded = true;

    bool isFirstFrameRendered = false;
    bool isFirstTimeComputed = false;

    void Awake () {
        tag = "InfluenceSystem";
        Units = new List<Influencer>[_NumberOfPlayers];
        Buildings = new List<Influencer>[_NumberOfPlayers];
        MapResources = new List<Influencer>();

        militaryMaps = new InfluenceMap[_NumberOfPlayers];
        economyMaps = new InfluenceMap[_NumberOfPlayers];
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        

        for (int i = 0; i < _NumberOfPlayers; i++)
        {
            Units[i] = new List<Influencer>();
            Buildings[i] = new List<Influencer>();
        }
        for (int i = 0; i < _NumberOfPlayers; i++)
        {
            militaryMaps[i] = new InfluenceMap(mapPlanes[i], _Size, Units[i], Units[_NumberOfPlayers-1-i]);
            militaryMaps[i].numberOfPlayers = _NumberOfPlayers;
            militaryMaps[i].width = _Size.x;
            militaryMaps[i].height = _Size.y;

            economyMaps[i] = new InfluenceMap(mapPlanes[i], _Size, Buildings[i], Buildings[_NumberOfPlayers - 1 - i], MapResources);
            economyMaps[i].numberOfPlayers = _NumberOfPlayers;
            economyMaps[i].width = _Size.x;
            economyMaps[i].height = _Size.y;
        }
    }

	void Update () {
        if(hasMilitaryCoroutineEnded)
            StartCoroutine("UpdateMilitaryMap");
	}

    void UpdateBuildingInfluence(int player)
    {
        UpdateMap(economyMaps, false, player, true);
    }

    IEnumerator UpdateMilitaryMap()
    {

        hasMilitaryCoroutineEnded = false;

        yield return new WaitForSeconds(1);
        for (int i = 0; i < _NumberOfPlayers; i++)
        {
            //if(isFirstTimeComputed) militaryMaps[i].WaitForJobComplete();
            UpdateMap(militaryMaps, true, i, false);
        }
        hasMilitaryCoroutineEnded = true;
        isFirstFrameRendered = true;
        isFirstTimeComputed = true;
    }

    void UpdateMap(InfluenceMap[] maps, bool isMilitary, int player, bool drawMap = false)
    {
        foreach (InfluenceMap map in maps)
        {
            map.Update();
        }
        if(drawMap)
            DrawMap(0, maps);
        ApplyMap(maps, isMilitary, player);
    }

    void LateUpdate()
    {
        GetCellInfluence();
        if (!isFirstFrameRendered)
        {
            transform.position = GetCenter(GameObject.Find("Hex Grid"));
            transform.position += new Vector3(0, _MapHeight,0);
        }
    }

    void DrawMap(int player, InfluenceMap[] mapArray) //player -> 0-n
    {
        mapPlanes[player].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", mapArray[player].CurrentTexture);
    }

    void ApplyMap(InfluenceMap[] mapArray, bool isMilitary, int player)
    {
        HexCell[] cells = grid.cells;
        List<Color>[] buffer = mapArray[player].GetColorBuffer(grid);
        for (int i = 0; i < cells.Length; i++)
        {
            Color averageInfluence = AverageColor(buffer[cells[i].Index]);
            if(isMilitary) cells[i].influenceInfo[player].MilitaryInfluence = averageInfluence;
            else cells[i].influenceInfo[player].EconomicInfluence = averageInfluence;
        }
    }

    Color AverageColor(List<Color> colorList)
    {
        Color averageColor = Color.black;
        float r = 0, g = 0, b = 0, a = 0;
        foreach (Color c in colorList)
        {
            r += c.r;
            g += c.g;
            b += c.b;
            a += c.a;
        }
        if (colorList.Count == 0)
        {
            //print("ColorList is Empty");
            return new Color(0, 0, 0, 0);
        }
        else
        {
            //print("(" + r / colorList.Count + "," + g / colorList.Count + "," + b / colorList.Count + "," + a / colorList.Count + ")");
            averageColor = new Color(r / colorList.Count, g / colorList.Count, b / colorList.Count, a / colorList.Count);
            //print(averageColor);
            return averageColor;
        }
        
    }

    Vector3 GetCenter(GameObject o)
    {
		Debug.Log ("O es: " + o);
        Vector3 center = Vector3.zero;
        MeshRenderer[] renderers = o.GetComponentsInChildren<MeshRenderer>();
        int count = 0;
        foreach (MeshRenderer renderer in renderers)
        {
            if(renderer.name == "Terrain")
            {
                center += renderer.bounds.center;
                count++;
            }
        }
        center /= count;
		Debug.Log ("El centro de la grid es: "+center);

        return center;
    }

    void GetCellInfluence()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell != null)
                //print("Influence: " + cell.influence);
                print("IsFrontier: " + cell.influenceInfo[1].IsBuildingFrontier);
            else
                print("NULL");
        }
    }
}
