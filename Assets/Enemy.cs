using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    int madera, piedra, comida, aldeanos;

    bool empezado = false;

    public HexGrid hexGrid;

    public enum Unidades { soldado, arquero, jinete }

    public enum Edificio { aserradero, mina, molino, cuartel, arqueria, caballeria }

    Vector3 castilloInicial = new Vector3(192, 15, 180); 
    Vector3 segundoCastillo = new Vector3(295, 15, 120);
    HexCell[] castillos = new HexCell[2];

    Unidades Unidad_deseada = Unidades.jinete;

    int[] recursos_turno = new int[3];              //POSICIÓN 0 MADERA, 1 PIEDRA, 2 COMIDA
    int[] recursos_tropas = new int[3];

    int[] offensive_building = new int[3];          //POSICIÓN 0 SOLDADO, 2 ARQUERO, 3 JINETE
    int castillo = 0;
    int edificio;

    HexGrid HexGrid;

    public bool Empezado
    {
        get
        {
            return empezado;
        }

        set
        {
            empezado = value;
        }
    }

    //   public HexCell Castillo;


    // Use this for initialization
    void Start()
    {
        madera = 100;
        piedra = 100;
        comida = 100;
        aldeanos = 1;
        castillos[0] = hexGrid.GetCell(5, 12);
        castillos[1] = hexGrid.GetCell(13, 8);


    }

    // Update is called once per frame
    void Update()
    {
        if (empezado == false) { return; }
        if (aldeanos < 1)
        {
            comida -= 50;
            aldeanos++;
        }
        else {
            if (castillo < 1 || edificio >= castillo * 6)
            {
                buildCastillo(castillo);

            }
            else
            {
                if (recursos_turno[0] == 0 && haveResources(Edificio.molino))
                {
                    build(Edificio.molino);
                }
                if (recursos_turno[1] == 0 && haveResources(Edificio.aserradero))
                {
                    build(Edificio.aserradero);
                }
                if (recursos_turno[2] == 0 && haveResources(Edificio.mina))
                {
                    build(Edificio.mina);
                }

                if (Unidad_deseada == Unidades.jinete && haveResources(Edificio.caballeria))
                {
                    build(Edificio.caballeria);
                }
                else if (Unidad_deseada == Unidades.soldado && haveResources(Edificio.cuartel))
                {
                    build(Edificio.cuartel);

                }
                else if (Unidad_deseada == Unidades.arquero && haveResources(Edificio.arqueria))
                {
                    build(Edificio.arqueria);
                }
            }
        }
    }
    bool haveResources(Edificio name)
    {
        switch (name)
        {
            case Edificio.aserradero:            
                if (piedra > 50 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.mina:             
                if (madera > 50 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.molino:            
                if (madera > 25 && piedra > 25 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.cuartel:             
                if (madera > 35 && piedra > 35 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.arqueria:             
                if (madera > 35 && piedra > 35 && aldeanos >= 1)
                {
                    return true;
                }
                break;
            case Edificio.caballeria:             
                if (madera > 35 && piedra >35 && aldeanos >= 1)
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
        foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
        {
            if (castillos[castillo-1].GetNeighbor(direction).SpecialIndex == 0)
            {
                switch (name)
                {
                    case Edificio.aserradero:
                        piedra -= 50;
                        aldeanos--;
                        break;
                    case Edificio.mina:
                        madera -= 50;
                        aldeanos--;
                        break;
                    case Edificio.molino:
                        piedra -= 25;
                        madera -= 25;
                        aldeanos--;
                        break;
                    case Edificio.cuartel:
                        piedra -= 35;
                        madera -= 35;
                        aldeanos--;
                        break;
                    case Edificio.arqueria:
                        piedra -= 35;
                        madera -= 35;
                        aldeanos--;
                        break;
                    case Edificio.caballeria:
                        piedra -= 35;
                        madera -= 35;
                        aldeanos--;
                        break;
                    default:
                        break;
                }
                //edificio++;
            }
        }
    }

       

    void buildCastillo(int numero)
    {

        castillos[castillo].SpecialIndex = 1;
        
        foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
        {
            castillos[castillo].GetNeighbor(direction).Walled = true;
            castillos[castillo].Owner = 2;
        }

        castillos[castillo].Walled = true;
        castillos[castillo].Owner = 2;

        castillo++;
        aldeanos--;
        //castillos[numero].SpecialIndex = 1;
    }
}


