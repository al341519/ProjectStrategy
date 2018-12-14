using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMapSystem : MonoBehaviour {

    Influencer[] influencers;
    public GameObject[] mapPlanes;

    public List<Influencer>[] Units { get; set; }
    public List<Influencer>[] Buildings { get; set; }

    InfluenceMap[] maps;
    HexGrid grid;

    public Vector2Int _Size = new Vector2Int(100, 66);

    public int _NumberOfPlayers = 2;
    public int _MapHeight = 100;

    bool isFirstFrameRendered = false;

    void Awake () {
        tag = "InfluenceSystem";
        Units = new List<Influencer>[_NumberOfPlayers];
        Buildings = new List<Influencer>[_NumberOfPlayers];
        maps = new InfluenceMap[_NumberOfPlayers];
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        

        for (int i = 0; i < _NumberOfPlayers; i++)
        {
            Units[i] = new List<Influencer>();
            Buildings[i] = new List<Influencer>();
        }
        for (int i = 0; i < _NumberOfPlayers; i++)
        {
            maps[i] = new InfluenceMap(mapPlanes[i], _Size, Units[i], Units[_NumberOfPlayers-1-i], Buildings[i]);
            maps[i].numberOfPlayers = _NumberOfPlayers;
            maps[i].width = _Size.x;
            maps[i].height = _Size.y;
        }
    }

	void Update () {
        foreach (InfluenceMap map in maps)
        {
            map.Update();
        }
        DrawMap(0);
        ApplyMap(0);
	}

    void LateUpdate()
    {
        if (!isFirstFrameRendered)
        {
            transform.position = GetCenter(GameObject.Find("Hex Grid"));
            transform.position += new Vector3(0, _MapHeight,0);
            isFirstFrameRendered = true;
        }
        GetCellInfluence();
    }

    void DrawMap(int player) //player -> 0-n
    {
        mapPlanes[player].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", maps[player].CurrentTexture);
    }

    void ApplyMap(int mapIndex)
    {
        HexCell[] cells = grid.cells;
        List<Color>[] buffer = maps[mapIndex].GetBufferColor(grid);
        for (int i = 0; i < cells.Length; i++)
        {
            Color averageInfluence = AverageColor(buffer[cells[i].Index]);
            cells[i].influence = averageInfluence;
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
        //if (colorList.Count == 0) print("ColorList is Empty"); 
        //print("(" + r / colorList.Count + "," + g / colorList.Count + "," + b / colorList.Count + "," + a / colorList.Count + ")");
        averageColor = new Color(r/colorList.Count, g / colorList.Count, b / colorList.Count, a / colorList.Count);
        //print(averageColor);
        return averageColor;
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
        if (Input.GetMouseButton(0))
        {
            HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell!=null)
                print("Influence: " + cell.influence);
        }
    }
}
