using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edificio : MonoBehaviour {

    int vida;
    int madera;
    int trigo;
    int metal;
    int aldeano;

    
    string id;

    GameManager game;
    int[] recursos;

    public GameObject aserraderoPrefab;
    public GameObject forjaPrefab;
    public GameObject molinoPrefab;
    public GameObject infanteriaPrefab;
    public GameObject arqueriaPrefab;
    public GameObject caballeriaPrefab;
    public GameObject castilloPrefab;

    public void Start()
    {
        game = GetComponent<GameManager>();

    }

    public void newEdificio(string identificador, int x, int y)
    {
        Debug.Log("identificador: " + identificador + " x: " + x + " y: " + y);
        transform.position = new Vector3 (x,0,y);
        id = identificador;

        switch (id)
        {
            case "aserradero":
                vida = 50;
                madera = 0;
                trigo = 0;
                metal = 0;
                aldeano = 1;
                break;
            case "forja":
                vida = 50;
                madera = 0;
                trigo = 0;
                metal = 0;
                aldeano = 1;
                break;
            case "molino":
                vida = 50;
                madera = 0;
                trigo = 0;
                metal = 0;
                aldeano = 1;
                break;
            case "infantería":
                vida = 50;
                madera = 0;
                trigo = 0;
                metal = 0;
                aldeano = 1;
                break;
            case "arquería":
                vida = 50;
                madera = 0;
                trigo = 0;
                metal = 0;
                aldeano = 1;
                break;
            case "caballería":
                vida = 50;
                madera = 0;
                trigo = 0;
                metal = 0;
                aldeano = 1;
                break;
            case "castillo":
                vida = 100;
                madera = 0;
                trigo = 0;
                metal = 0;
                aldeano = 0;
                break;
            default:
                break;
        }

        crearEdificio();
    }

    void crearEdificio()
    {
        if (recursos[0] >= madera &&
            recursos[1] >= trigo &&
            recursos[2] >= metal &&
            recursos[3] >= aldeano)
        {
            Debug.Log("Recursos0: " + recursos[0]);


            switch (id)
            {
                case "aserradero":
                    Instantiate(aserraderoPrefab);
                    break;
                case "forja":
                    Instantiate(forjaPrefab);
                    break;
                case "molino":
                    Instantiate(molinoPrefab);
                    break;
                case "infantería":
                    Instantiate(infanteriaPrefab);
                    break;
                case "arquería":
                    Instantiate(arqueriaPrefab);
                    break;
                case "caballería":
                    Instantiate(caballeriaPrefab);
                    break;
                case "castillo":
                    Instantiate(castilloPrefab);
                    break;
                default:
                    break;
            }
        }

        else
        {
            Debug.Log("NO HAY SUFICIENTES MATERIALES");
        }
    }
}
