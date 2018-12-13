using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {
    int madera, piedra, comida, aldeanos;

    bool turno = false;

    public enum Unidades {soldado, arquero, jinete}

    public enum Edificio{aserradero, mina, molino, cuartel, arqueria, caballeria}

    Unidades Unidad_deseada = Unidades.jinete;

    int[] recursos_turno = new int[3];              //POSICIÓN 0 MADERA, 1 PIEDRA, 2 COMIDA
    int[] recursos_tropas = new int[3];

    int[] offensive_building = new int[3];          //POSICIÓN 0 SOLDADO, 2 ARQUERO, 3 JINETE
    int castillos;
    int edificios;

    HexGrid HexGrid;
    HexCell Castillo;


	// Use this for initialization
	void Start () {
        madera = 100;
        piedra = 100;
        comida = 100;
        aldeanos = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (aldeanos < 1) {
            comida -= 50;
            aldeanos++;
        }
        if (castillos <= 1 || edificios >= castillos * 6) {
            castillos++;
            aldeanos--;
        }
        else {
            if (Unidad_deseada == Unidades.jinete && haveResources(Edificio.caballeria)) {
                foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                {
                    if (Castillo.GetNeighbor(direction).SpecialIndex != 0) {
                        build(Edificio.caballeria);
                    }
                }
            }
            else if (Unidad_deseada == Unidades.soldado && haveResources(Edificio.cuartel))
            {
                foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                {
                    if (Castillo.GetNeighbor(direction).SpecialIndex == 0)
                    {
                        build(Edificio.cuartel);
                        break;
                    }
                }
            }
            else if (Unidad_deseada == Unidades.arquero && haveResources(Edificio.arqueria))
            {
                foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                {
                    if (Castillo.GetNeighbor(direction).SpecialIndex == 0)
                    {
                        build(Edificio.arqueria);
                        break;
                    }
                }
            }
            


        }


         



	}
    bool haveResources(Edificio name)
    {

        switch (name)
        {
            case Edificio.aserradero:             //Castillo
                if(piedra > 50 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.mina:             // "aserradero"
                if (madera > 50 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.molino:             // "forja":
                if (madera > 25 && piedra > 25 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.cuartel:             //"molino":
                if (madera > 25 && piedra > 25 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.arqueria:             //"infantería":
                if (madera > 25 && piedra > 25 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.caballeria:             //"arquería":
                if (madera > 25 && piedra > 25 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            default:
                break;        
        }
        return false;
    }


    void build(Edificio name) {

    }
}
