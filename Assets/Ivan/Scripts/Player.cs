using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    int id; //Identificador de que jugador es
    HexGrid hexGrid;
    HexCell cell;
    Edificio edificio;

    int madera;
    int metal;
    int trigo;
    int aldeano;

    int maderaEd;
    int metalEd;
    int trigoEd;
    int aldeanoEd;

    public int Madera
    {
        get
        {
            return madera;
        }

        set
        {
            madera = value;
        }
    }

    public int Metal
    {
        get
        {
            return metal;
        }

        set
        {
            metal = value;
        }
    }

    public int Trigo
    {
        get
        {
            return trigo;
        }

        set
        {
            trigo = value;
        }
    }

    public int Aldeano
    {
        get
        {
            return aldeano;
        }

        set
        {
            aldeano = value;
        }
    }

    public Player (int id) {
        this.id = id;
        madera = 1000;
        metal = 1000;
        trigo = 1000;
        aldeano = 6;
	}

    public bool construir(int id)
    {
            
        switch (id)
        {
            case 1:             //Castillo
                maderaEd = 10;
                trigoEd = 10;
                metalEd = 10;
                aldeanoEd = 1;
                break;
            case 2:             // "aserradero"
                maderaEd = 0;
                trigoEd = 0;
                metalEd = 50;
                aldeanoEd = 1;
                break;
            case 3:             // "forja":
                maderaEd = 50;
                trigoEd = 0;
                metalEd = 0;
                aldeanoEd = 1;
                break;
            case 4:             //"molino":
                maderaEd = 50;
                trigoEd = 0;
                metalEd = 50;
                aldeanoEd = 1;
                break;
            case 5:             //"infantería":
                maderaEd = 0;
                trigoEd = 50;
                metalEd = 0;
                aldeanoEd = 1;
                break;
            case 6:             //"arquería":
                maderaEd = 50;
                trigoEd = 50;
                metalEd = 0;
                aldeanoEd = 1;
                break;
            case 7:             //"caballería":
                maderaEd = 50;
                trigoEd = 50;
                metalEd = 50;
                aldeanoEd = 1;
                break;
            default:
                break;
        }

        if (madera >= maderaEd && metal >= metalEd
            && trigo >= trigoEd && aldeano >= aldeanoEd)
        {
            updateRecursos(maderaEd, metalEd, trigoEd, aldeanoEd);
            return true;
        }
        else
        {
            return false;
        
        }
    }

    void updateRecursos(int maderaEd, int metalEd, int trigoEd, int aldeanoEd)
    {
        madera -= maderaEd;
        metal -= metalEd;
        trigo -= trigoEd;
        aldeano -= aldeanoEd;
    }
}
