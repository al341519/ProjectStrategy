using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour {

    public GameObject UIMenu;
    public GameObject UIMain;

    public GameObject UIInicio;
    public GameObject UIRecursos;
    public GameObject UIAtaque;
    public GameObject UIDefensa;
    public GameObject UITropa;
    public GameObject Soldado;
    public GameObject Arquero;
    public GameObject Caballero;

    public GameObject UIEspecial;
    HexGrid grid;
    HexCell cell;

    public void Start()
    {
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
    }

    public void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (grid.GetCell(ray) != null)
            { 
                cell = grid.GetCell(ray);
            }
            if (cell.edificio.name == "Castillo(Clone)")
            {
                Debug.Log("PULSADO CASTILLO");
                OpenInicio();
            }
            else if (cell.edificio.name == "Infanteria(Clone)")
            {
                Debug.Log("Infanteria");
                OpenTropa1();
            }
            else if (cell.edificio.name == "Arqueria(Clone)")
            {
                Debug.Log("Arqueria");
                OpenTropa2();
            }
            else if (cell.edificio.name == "Caballeria(Clone)")
            {
                Debug.Log("Caballeria");
                OpenTropa3();
            }

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Inicio")
                {
                    Debug.Log("Inicio");
                    OpenInicio();
                }
                else if (hit.transform.name == "Recursos")
                {
                    Debug.Log("Recursos");
                    OpenRecursos();
                }
                else if (hit.transform.name == "Ataque")
                {
                    Debug.Log("Ataque");
                    OpenAtaque();
                }
                else if (hit.transform.name == "Defensa")
                {
                    Debug.Log("Defensa");
                    OpenDefensa();
                }
                else if (hit.transform.name == "Otros")
                {
                    Debug.Log("Otros");
                    OpenOtros();
                }
                
                else if (hit.transform.name == "Salir")
                {
                    Debug.Log("SALIR");
                    Application.Quit();
                }
                else
                {
                    Debug.Log("CLOSED");
                    //CloseMenu();
                }
            }
        }
    }

    public void OpenInicio()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(true);
        UIInicio.SetActive(true);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIEspecial.SetActive(false);
    }


    public void OpenRecursos()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(true);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(true);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenAtaque()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(true);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(true);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenDefensa()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(true);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(true);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenTropa1()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(false);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(true);
        Soldado.SetActive(true);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenTropa2()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(false);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(true);
        Soldado.SetActive(false);
        Arquero.SetActive(true);
        Caballero.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenTropa3()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(false);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(true);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(true);
        UIEspecial.SetActive(false);
    }

    public void OpenOtros()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(true);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIEspecial.SetActive(true);
    }

    public void CloseMenu()
    {
        UIMain.SetActive(false);
        UIMenu.SetActive(false);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIEspecial.SetActive(false);
    }

}
