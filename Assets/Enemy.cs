using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    int madera, piedra, comida, aldeanos;

    bool turno = false;

    public enum Unidades { soldado, arquero, jinete }

    public enum Edificio { aserradero, mina, molino, cuartel, arqueria, caballeria }

    Vector3 castilloInicial = new Vector3(0.5f,0f, 0.5f);
    Vector3 segundoCastillo = new Vector3(2, 0, 2);
    HexCell[] castillos = new HexCell[2];

    Unidades Unidad_deseada = Unidades.jinete;

    int[] recursos_turno = new int[3];              //POSICIÓN 0 MADERA, 1 PIEDRA, 2 COMIDA
    int[] recursos_tropas = new int[3];

    int[] offensive_building = new int[3];          //POSICIÓN 0 SOLDADO, 2 ARQUERO, 3 JINETE
    int castillo = 0;
    int edificio;

    HexGrid HexGrid;
    //   public HexCell Castillo;


    // Use this for initialization
    void Start()
    {
        madera = 100;
        piedra = 100;
        comida = 100;
        aldeanos = 1;
        castillos[0] = HexGrid.GetCell(castilloInicial);
        castillos[1] = HexGrid.GetCell(segundoCastillo);


    }

    // Update is called once per frame
    void Update()
    {
        if (aldeanos < 1)
        {
            comida -= 50;
            aldeanos++;
        }
        else {
            if (castillo <= 1 || edificio >= castillo * 6)
            {
                castillo++;
                aldeanos--;
                Debug.Log("TEST");
                buildCastillo(0);
                Debug.Log("TEST2");

            }
            else
            {
                if (recursos_turno[0] == 0 && haveResources(Edificio.aserradero))
                {
                    build(Edificio.aserradero);
                }
                if (recursos_turno[1] == 0 && haveResources(Edificio.mina))
                {
                    build(Edificio.mina);
                }
                if (recursos_turno[2] == 0 && haveResources(Edificio.molino))
                {
                    build(Edificio.molino);
                }

                if (Unidad_deseada == Unidades.jinete && haveResources(Edificio.caballeria))
                {
                    foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        if (castillos[0].GetNeighbor(direction).SpecialIndex != 0)
                        {
                            build(Edificio.caballeria);
                        }
                    }
                }
                else if (Unidad_deseada == Unidades.soldado && haveResources(Edificio.cuartel))
                {
                    foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        if (castillos[0].GetNeighbor(direction).SpecialIndex == 0)
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
                        if (castillos[0].GetNeighbor(direction).SpecialIndex == 0)
                        {
                            build(Edificio.arqueria);
                            break;
                        }
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
                if (piedra > 50 && aldeanos >= 1)
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


    void build(Edificio name)
    {
        edificio++;
    }

    void buildCastillo(int numero)
    {
        castillos[numero].SpecialIndex = 1;
    }
}


