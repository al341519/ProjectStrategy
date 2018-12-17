using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Influencer : MonoBehaviour {

    public int _InfluenceRadius = 30;
    public float _InfluencePower = 1;
    public float _InfluencePropagationRatio = 0.9f;

    public int _Team = 1; //1-n
    public enum InfluencerType { Unit, Building, Resource}
    public InfluencerType type;

    InfluenceMapSystem system;
    HexGrid grid;

    public bool IsVisible
    {
        get
        {
            if(type == InfluencerType.Resource)
            {
                return grid.GetCell(position).IsExplored;
            }
            else
            {
                return grid.GetCell(position).IsVisible;
            }
        }
    }

    Vector3 position;
    //Color color;

    //getters and setters

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    //Functions

    void Start()
    {
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        system = GameObject.FindGameObjectWithTag("InfluenceSystem").GetComponent<InfluenceMapSystem>();
        switch (type)
        {
            case InfluencerType.Building:
                system.Buildings[_Team-1].Add(this);
                break;
            case InfluencerType.Unit:
                system.Units[_Team-1].Add(this);
                break;
            case InfluencerType.Resource:
                system.MapResources.Add(this);
                break;
        }
        //system.Units[_Team].Add(this);
    }

    /*private void OnDestroy()
    {
        switch (type)
        {
            case InfluencerType.Building:
                system.Buildings[_Team-1].Remove(this);
                break;
            case InfluencerType.Unit:
                system.Buildings[_Team - 1].Remove(this);
                break;
            case InfluencerType.Resource:
                system.Buildings[_Team - 1].Remove(this);
                break;
        }
    }*/

}
