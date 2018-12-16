using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour {

    public GameObject UIMenu;
    public GameObject UIMain;

    public GameObject UICastillo;
    public GameObject UIInicio;
    public GameObject UIRecursos;
    public GameObject UIAtaque;
    public GameObject UIDefensa;
    public GameObject UITropa;
    public GameObject Soldado;
    public GameObject Arquero;
    public GameObject Caballero;

    public GameObject UIStart;
    HexGrid grid;
    HexCell cell;

    public void Start()
    {
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
    }

    public void Update()
    {
        Debug.Log(UIRecursos.activeSelf);


        if (Input.GetMouseButtonDown(0))
        {   
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            cell = grid.GetCell(ray);

            if (cell.SpecialIndex == 1 && cell.Owner == 1)
            {
                Debug.Log("PULSADO CASTILLO");
                OpenCastillo();
            }
            else if (cell.SpecialIndex == 5 && cell.Owner == 1)
            {
                Debug.Log("Infanteria");
                OpenTropa1();
            }
            else if (cell.SpecialIndex == 6 && cell.Owner == 1)
            {
                Debug.Log("Arqueria");
                OpenTropa2();
            }
            else if (cell.SpecialIndex == 7 && cell.Owner == 1)
            {
                Debug.Log("Caballeria");
                OpenTropa3();
            }

            
        }
    }


    public void OpenCastillo()
    {
        UIMain.SetActive(true);
        UIMenu.SetActive(false);
        UICastillo.SetActive(true);
        UIInicio.SetActive(true);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIStart.SetActive(false);
    }


    public void OpenInicio()
    {
        
        UIMain.SetActive(false);
        UIMenu.SetActive(true);
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        Soldado.SetActive(false);
        Arquero.SetActive(false);
        Caballero.SetActive(false);
        UIStart.SetActive(false);
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
        UIStart.SetActive(false);
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
        UIStart.SetActive(false);
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
        UIStart.SetActive(false);
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
        UIStart.SetActive(false);
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
        UIStart.SetActive(false);
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
        UIStart.SetActive(false);
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
        UIStart.SetActive(true);
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
        UIStart.SetActive(false);
    }

}
