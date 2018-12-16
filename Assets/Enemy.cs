using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Enemy : MonoBehaviour
{
    int madera, piedra, comida, aldeanos;

    bool empezado = false;

    public HexGrid hexGrid;

    public enum Unidades { soldado, arquero, jinete }

    public enum Edificio { castillo, aserradero, mina, molino, cuartel, arqueria, caballeria }

    HexCell[] castillos = new HexCell[2];

    Unidades Unidad_deseada = Unidades.jinete;

    int[] recursos_turno = new int[3];              //POSICIÓN 0 MADERA, 1 PIEDRA, 2 COMIDA
    int[] recursos_tropas = new int[3];

    int[] offensive_building = new int[3];          //POSICIÓN 0 SOLDADO, 2 ARQUERO, 3 JINETE
    int castillo = 0;
    int edificio;

    float timerTurno = HexMetrics.tiempo;

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
        madera = 395;
        piedra = 395;
        comida = 300;
        aldeanos = 1;
        castillos[0] = hexGrid.GetCell(5, 12);
        castillos[1] = hexGrid.GetCell(13, 8);


    }

    // Update is called once per frame
    void Update()
    {
        if (empezado == false) { return; }

        timerTurno += Time.deltaTime;

        if (timerTurno < HexMetrics.tiempo) { return; }                 //Cada segundo realiza una acción

        UpdateResources();
        chooseUnit();
        chooseBuilding();

        timerTurno = 0f;
    }

    void chooseUnit() {

    }


    bool haveResources(Edificio name)
    {
        switch (name)
        {
            case Edificio.castillo:
                if (madera > 300 && piedra > 300 && comida > 200 && aldeanos >= 1) {
                    return true;
                }
                break;
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
            HexCell cell = castillos[castillo - 1].GetNeighbor(direction);
            if (cell.SpecialIndex == 0)
            {
                switch (name)
                {
                    case Edificio.aserradero:
                        piedra -= 50;
                        aldeanos--;
                        cell.SpecialIndex = 2;
                        recursos_turno[0]++;
                        break;
                    case Edificio.mina:
                        madera -= 50;
                        aldeanos--;
                        cell.SpecialIndex = 3;
                        recursos_turno[1]++;
                        break;
                    case Edificio.molino:
                        piedra -= 25;
                        madera -= 25;
                        aldeanos--;
                        cell.SpecialIndex = 4;
                        recursos_turno[2]++;
                        break;
                    case Edificio.cuartel:
                        piedra -= 35;
                        madera -= 35;
                        cell.SpecialIndex = 5;
                        offensive_building[0]++;
                        aldeanos--;
                        break;
                    case Edificio.arqueria:
                        piedra -= 35;
                        madera -= 35;
                        cell.SpecialIndex = 6;
                        offensive_building[1]++;
                        aldeanos--;
                        break;
                    case Edificio.caballeria:
                        piedra -= 35;
                        madera -= 35;
                        cell.SpecialIndex = 7;
                        offensive_building[2]++;
                        aldeanos--;
                        break;
                    default:
                        break;
                }
                cell.Owner = 2;
                edificio++;
                break;
            }
        }
    }

       

    void buildCastillo(int numero)
    {
        castillos[castillo].SpecialIndex = 1;
        
       	foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
        {
      //      castillos[castillo].GetNeighbor(direction).Walled = true;
            castillos[castillo].Owner = 2;
        }

        castillos[castillo].Walled = true;
        castillos[castillo].Owner = 2;
        madera -= 300;
        piedra -= 300;
        comida -= 200;
        aldeanos--;
        castillo++;
        aldeanos--;
        //castillos[numero].SpecialIndex = 1;
    }

    void UpdateResources() {
        madera += recursos_turno[0] * 10;
        piedra += recursos_turno[1] * 10;
        comida += recursos_turno[2] * 10;
    }

    void chooseBuilding() {
        if (aldeanos < 1 && comida >= 30)
        {
            comida -= 30;
            aldeanos++;
        }
        else {
            if ((castillo < 1 || edificio >= castillo * 6)&& haveResources(Edificio.castillo))
            {
                buildCastillo(castillo);
            }        
            else
            {
                if ((recursos_turno[2] == 0 || recursos_turno[2] <= castillo-1) && haveResources(Edificio.molino))
                {
                    build(Edificio.molino);
                }
                if ((recursos_turno[0] == 0 || recursos_turno[0] <= castillo - 1) && haveResources(Edificio.aserradero))
                {
                    build(Edificio.aserradero);
                }
                if ((recursos_turno[1] == 0 || recursos_turno[1] <= castillo - 1 ) && haveResources(Edificio.mina))
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

}




