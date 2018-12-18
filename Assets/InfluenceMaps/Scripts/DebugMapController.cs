using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMapController : MonoBehaviour {

    InfluenceMapSystem system;

    void Start()
    {
        system = GetComponent<InfluenceMapSystem>();
    }

    void Update () {
        //Player
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1
        {
            system.debugPlayer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //2
        {
            system.debugPlayer = 1;
        }

        //MapType
        if (Input.GetKeyDown(KeyCode.M)) //M --> militaryMap
        {
            system.debugMap = 0;
        }
        else if (Input.GetKeyDown(KeyCode.N)) //N --> economicMap
        {
            system.debugMap = 1;
        }
    }
}
