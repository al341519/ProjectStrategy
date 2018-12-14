using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iniGame : MonoBehaviour {

    int[] recursos;

    public Texture2D iconCastle;
    public Texture2D iconHoverCastle;
    public Texture2D iconMine;
    public Texture2D iconHoverMine;
    public Texture2D iconInfanteria;
    public Texture2D iconHoverInfanteria;

    // public GameObject Ediifios;
    public Texture2D IconContainer;

    bool casillaElegida;
    bool construir;
    bool start;

    int x;
    int y;

    int madera;
    int trigo;
    int metal;
    int aldeanos;
    int vida;

    private void Awake()
    {
        recursos = new int[4];
        recursos[0] = 1000;
        recursos[1] = 1000;
        recursos[2] = 1000;
        recursos[3] = 1000;

        casillaElegida = false;
        construir = false;
        start = true;
    }

    private void OnGUI()
    {
        if (casillaElegida)
        {
            GUIStyle container = new GUIStyle();
            container.normal.background = IconContainer;

            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.width / 2 - 200, 50, 50), "", container);

            //Generate unit buttons
            int offset = 48;
            int j = 0;
            string id;


            for (int i = 0; i < 1; i++)
            {
                GUIStyle Icon = new GUIStyle();

                id = "castillo";
                x = 0;
                y = 0;
                Icon.normal.background = iconCastle;
                Icon.hover.background = iconHoverCastle;

                if (GUI.Button(new Rect(Screen.width / 2 - 199 + (offset * j), Screen.width / 2 - 200, 46, 39), "", Icon))
                {
                    Debug.Log("Pulsado");
                    Debug.Log("ID: " + id);

                    //CREAR EDIFICIO
                    Edificio edificio = GetComponent<Edificio>();
                    edificio.newEdificio(id, x, y);
                    casillaElegida = false;
                   
                }

                j++;
            }
        }
        else if (construir)
        {
            GUIStyle container = new GUIStyle();
            container.normal.background = IconContainer;

            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height - 40, 400, 50), "", container);

            //Generate unit buttons
            int offset = 48;
            int j = 0;
            string id;


            for (int i = 0; i < 2; i++)
            {
                GUIStyle Icon = new GUIStyle();
                if (i == 0)
                {
                    id = "forja";
                    x = 6;
                    y = 0;
                    Icon.normal.background = iconMine;
                    Icon.hover.background = iconHoverMine;
                }
                else
                {
                    id = "infantería";
                    x = 3;
                    y = -6;
                    Icon.normal.background = iconInfanteria;
                    Icon.hover.background = iconHoverInfanteria;
                }

                if (GUI.Button(new Rect(Screen.width / 2 - 199 + (offset * j), Screen.height - 39, 46, 39), "", Icon))
                {
                    Debug.Log("Pulsado");
                    Debug.Log("ID: " + id);

                    Edificio edificio = GetComponent<Edificio>();
                    edificio.newEdificio(id, x, y);

                }

                j++;
            }
        }
        /*else if (soldados)
        {
            GUIStyle container = new GUIStyle();
            container.normal.background = IconContainer;

            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.width / 2 - 200, 400, 50), "", container);

            //Generate unit buttons
            int offset = 48;
            int j = 0;
            string id;


            for (int i = 0; i < 2; i++)
            {
                GUIStyle Icon = new GUIStyle();
                id = "soldado";
                x = 0;
                y = 0;
                Icon.normal.background = iconCastle;
                Icon.hover.background = iconHoverCastle;

                if (GUI.Button(new Rect(Screen.width / 2 - 199 + (offset * j), Screen.height - 39, 46, 39), "", Icon))
                {
                    Debug.Log("Pulsado");
                    Debug.Log("ID: " + id);

                    Soldado soldado = GetComponent<Soldado>();
                    soldado.newSoldado(id, x, y);

                }

                j++;
            }
        }*/
    }

    // Update is called once per frame
    void Update () {
        //ELEGIR CASILLA
        if (start)
        {
            if (Input.GetMouseButtonDown(0))
            {
                casillaElegida = true;
                start = false;
            }
        }
        
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                /*//RAY CAST DE CASTILLO
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform.tag == "Castle")
                    {
                        construir = true;
                    }
                    else if (hit.transform.tag == "Infanteria")
                    {
                        soldados = true;
                    }
                }*/

                //construir = true;

            }
        }
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
