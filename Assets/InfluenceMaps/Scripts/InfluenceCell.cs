using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceCell{

    public Color EconomicInfluence { get; set; }

    public Color MilitaryInfluence { get; set; }

    public bool IsMilitaryFrontier
    {
        get
        {
            if (MilitaryInfluence.g > 0 && MilitaryInfluence.g < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsBuildingFrontier
    {
        get
        {
            if (EconomicInfluence.g > 0 && EconomicInfluence.g < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public InfluenceCell()
    {
        EconomicInfluence = new Color(0, 0, 0, 0);
        MilitaryInfluence = new Color(0, 0, 0, 0);
    }

}
