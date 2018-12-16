using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalomaClass : MonoBehaviour
{
    public float speedPidgeon;
    public HexGameUI uiGame;

    private float actionRange;
    private Transform objetivoPaloma;
    
    private HexCell lastCell;
    private HexGrid grid;
    private List<HexCell> listGrid;

	[SerializeField]
	private HexUnit unidadObjetivo;

    // Use this for initialization
    void Awake()
    {
        //La paloma debe buscar la unidad activa
        actionRange = this.transform.position.y + 10;
        uiGame = GameObject.Find("Game UI").GetComponent<HexGameUI>();
        grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();

    }

    // Update is called once per frame
    void Update()
    {
		if (!unidadObjetivo)
        {
            Destroy(this);
        }
        else {
            Vector3 observarA = new Vector3(objetivoPaloma.position.x, this.transform.position.y, objetivoPaloma.position.z);
            this.transform.LookAt(observarA);
            this.transform.Translate(new Vector3(0, 0, speedPidgeon * Time.deltaTime));

            if (distancePU() < actionRange)
            {
                //Debug.Log ("He llegado al sitio");
                if (unidadObjetivo)
                {
                    HexCell holaCell = unidadObjetivo.Location;
                    grid.ClearPath();
                    grid.FindPath(holaCell, lastCell, unidadObjetivo);
                    listGrid = grid.GetPath();
                    unidadObjetivo.Travel(listGrid);
                    Destroy(this.gameObject);
                }
                //unidadObjetivo.Travel

            }
        }
    }

    public void Objetivo(HexUnit unidad, List<HexCell> path)
    {
        objetivoPaloma = unidad.transform;
        unidadObjetivo = unidad;
        lastCell = path[path.Count - 1];
    }
    private float distancePU()
    {
        float realDistance = Mathf.Abs(Vector3.Distance(this.transform.position, objetivoPaloma.transform.position));
        return realDistance;
    }
}
