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

    bool isFirstFrameRendered = false;

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
            militaryMaps[i].numberOfPlayers = _NumberOfPlayers;
            militaryMaps[i].width = _Size.x;
            militaryMaps[i].height = _Size.y;
        }
    }

	void Update () {
        //UpdateMap(militaryMaps, true);
        UpdateMap(economyMaps, false);
	}

    void UpdateMap(InfluenceMap[] maps, bool isMilitary)
    {
        foreach (InfluenceMap map in maps)
        {
            map.Update();
        }
        DrawMap(0, maps);
        ApplyMap(0, maps, isMilitary);
    }

    void LateUpdate()
    {
        GetCellInfluence();
        if (!isFirstFrameRendered)
        {
            transform.position = GetCenter(GameObject.Find("Hex Grid"));
            transform.position += new Vector3(0, _MapHeight,0);
            isFirstFrameRendered = true;
        }
    }

    void DrawMap(int player, InfluenceMap[] mapArray) //player -> 0-n
    {
        mapPlanes[player].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", mapArray[player].CurrentTexture);
    }

    void ApplyMap(int mapIndex, InfluenceMap[] mapArray, bool isMilitary)
    {
        HexCell[] cells = grid.cells;
        List<Color>[] buffer = mapArray[mapIndex].GetColorBuffer(grid);
        for (int i = 0; i < cells.Length; i++)
        {
            Color averageInfluence = AverageColor(buffer[cells[i].Index]);
            if(isMilitary) cells[i].MilitaryInfluence = averageInfluence;
            else cells[i].EconomicInfluence = averageInfluence;

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
        return center;
    }

    void GetCellInfluence()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell != null)
                //print("Influence: " + cell.influence);
                print("IsFrontier: " + cell.IsFrontier);
            else
                print("NULL");
        }
    }
}
