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

    public bool IsVisible { get; set; }

    /*public Color Color
    {
        get
        {
            switch (type)
            {
                case InfluencerType.Unit:
                    return new Color(1, 0, 0, 1);
                case InfluencerType.Building:
                    return new Color(0, 0, 1, 1);
                default:
                    return new Color(0, 0, 0, 0);
            }
        }
    }*/

    //Functions

    void Start()
    {
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
                system.Units[_Team-1].Add(this);
                break;
        }
        IsVisible = true;
        //system.Units[_Team].Add(this);
    }

    void Update()
    {

    }
}
