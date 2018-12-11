using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager {

    public GameObject UIInicio;
    public GameObject UIRecursos;
    public GameObject UIAtaque;
    public GameObject UIDefensa;
    public GameObject UITropa;
    public GameObject UIEspecial;

    public void OpenInicio()
    {
        UIInicio.SetActive(true);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenRecursos()
    {
        UIInicio.SetActive(false);
        UIRecursos.SetActive(true);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenAtaque()
    {
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(true);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenDefensa()
    {
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(true);
        UITropa.SetActive(false);
        UIEspecial.SetActive(false);
    }

    public void OpenTropa()
    {
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(true);
        UIEspecial.SetActive(false);
    }

    public void OpenOtros()
    {
        UIInicio.SetActive(false);
        UIRecursos.SetActive(false);
        UIAtaque.SetActive(false);
        UIDefensa.SetActive(false);
        UITropa.SetActive(false);
        UIEspecial.SetActive(true);
    }
}
