using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Ed
{
    Castillo, Forja, Aserradero, Molino,
    Infanteria, Arqueria, Caballeria, Sierra, Mina,
    Torre, Paloma, Barricada,
    Arquero, Soldado, Caballero, Aldeano, NONE
}

public class Information : MonoBehaviour {
  
    HexCell cell;// { get; set; }

    public void SetCell(HexCell cello) {
        cell = cello;
    }

    public HexCell GetCell() { return cell; }

    Ed ed;
    string edificio;

    public void SetEdificio(int id)
    {
        switch (id)
        {
            case 1:             //Castillo
                ed = Ed.Castillo;
                break;
            case 2:             // "aserradero"
                ed = Ed.Aserradero;
                break;
            case 3:             // "forja":
                ed = Ed.Forja;
                break;
            case 4:             //"molino":
                ed = Ed.Molino;
                break;
            case 5:             //"infantería":
                ed = Ed.Infanteria;
                break;
            case 6:             //"arquería":
                ed = Ed.Arqueria;
                break;
            case 7:             //"caballería":
                ed = Ed.Caballeria;
                break;
            case 8:             //"torre":
                ed = Ed.Torre;
                break;
            case 9:             //"torre de palomas":
                ed = Ed.Paloma;
                break;
            case 10:             //"barricada":
                ed = Ed.Barricada;
                break;
            case 11:             //"mina":
                ed = Ed.Mina;
                break;
            case 12:             //"sierra":
                ed = Ed.Sierra;
                break;
            case 13:             //"arquero":
                ed = Ed.Arquero;
                break;
            case 14:             //"soldado":
                ed = Ed.Soldado;
                break;
            case 15:             //"caballero":
                ed = Ed.Caballero;
                break;
            case 16:             //"aldeano":
                ed = Ed.Aldeano;
                break;
            default:
                break;
        }
    }

   
}
