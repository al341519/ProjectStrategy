using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    int[] recursos;

    HexCell cell;

    int madera;
    int trigo;
    int metal;
    int aldeanos;
    int vida;

    int x;
    int y;

    public Texture2D IconContainer;
    public Texture2D iconMine;
    public Texture2D iconHoverMine;
    public Texture2D iconCastle;
    public Texture2D iconHoverCastle;

    public GameObject Ediifios;

    private void Awake()
    {
        recursos = new int[4];
        recursos[0] = 1000;
        recursos[1] = 1000;
        recursos[2] = 1000;
        recursos[3] = 1000;

    }

    private void OnGUI()
    {
        GUIStyle container = new GUIStyle();
        container.normal.background = IconContainer;

        GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height - 40, 400, 50), "", container);

        //Generate unit buttons
        int offset = 48;
        int j = 0;
        string id;
        

        for(int i = 0; i < 2; i++)
        {
            GUIStyle Icon = new GUIStyle();
            if (i == 0)
            {
                id = "forja";
                x = -3;
                y = 6;
                Icon.normal.background = iconMine;
                Icon.hover.background = iconHoverMine;
            }
            else
            {
                id = "castillo";
                x = 0;
                y = 0;
                Icon.normal.background = iconCastle;
                Icon.hover.background = iconHoverCastle;
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 199 + (offset * j), Screen.height - 39, 46, 39), "", Icon))
            {
                Debug.Log("Pulsado");
                Debug.Log("ID: " + id);
                //CREAR EDIFICIO


                Edificio edificio = GetComponent<Edificio>();
                edificio.newEdificio(id, x, y);
            }

            j++;
        }
    }

    void Update()
    {
        //Debug.Log( Time.time);
    }

    public int[] GetRecursos()
    {
        return recursos;
    }

    public void UpdateRecursos(int madera, int trigo, int metal, int aldeano)
    {
        recursos[0] -= madera;
        recursos[1] -= trigo;
        recursos[2] -= metal;
        recursos[3] -= aldeano;
    }

}
