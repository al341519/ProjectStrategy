using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Enemy : MonoBehaviour
{
    int madera, piedra, comida, aldeanos;

    bool empezado = false;
    bool createTroops = false;
    bool vuelve = false;
    bool alAtaque = false;

    //PRUEBA
    //GameObject enemy;

    public HexGrid hexGrid;

    public enum Unidades { soldado, arquero, jinete }

    public HexUnit[] enemyUnitPrefab = new HexUnit[3];

    public enum Edificio { castillo, aserradero, mina, molino, cuartel, arqueria, caballeria }

    HexCell[] castillos = new HexCell[2];
    HexCell[] tower = new HexCell[2];
    bool[] construido = new bool[2];
    List<HexCell> recruitUnitBuildings = new List<HexCell>();

    HexUnit[] units = new HexUnit[8];

    Unidades Unidad_deseada = Unidades.jinete;

    int[] recursos_turno = new int[3];              //POSICIÓN 0 MADERA, 1 PIEDRA, 2 COMIDA
    int[] recursos_tropas = new int[3];

    int[] offensive_building = new int[3];          //POSICIÓN 0 SOLDADO, 2 ARQUERO, 3 JINETE
    int castillo = 0;
    int edificio;
    int tropas;

    float timerTurno = HexMetrics.tiempo;

	private bool defensivo=true;
	private List<GameObject> unidadesEnem;
    private List<GameObject> unidadesAlly;
    HexCell[] castillosAlly;
    List<HexCell> castilloPosible;

    HexUnit enemyUnit;
    HexCell currentCell;
    HexCell targetCell;
    Vector3 pos;
    Vector3 iniPos;
    List<HexCell> listGrid;
    List<List<HexCell>> listas;

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


    // Use this for initialization
    void Start()
    {
        madera = 395;
        piedra = 395;
        comida = 300;
        aldeanos = 1;
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();

        castillos[0] = hexGrid.GetCell(11, 12);
        castillos[1] = hexGrid.GetCell(17, 8);


        tower[0] = hexGrid.GetCell(8, 9);
        tower[1] = hexGrid.GetCell(13, 3);
        


    }

    // Update is called once per frame
    void Update()
    {
        
   //     Debug.Log("AL ATAQUEEE --> "+ createTroops);
    //    Debug.Log("md: " + madera + " pd: " + piedra + " cmd: " + comida);
        if (empezado == false) { return; }

        timerTurno += Time.deltaTime;

        if (timerTurno < HexMetrics.tiempo) { return; }                 //Cada segundo realiza una acción

        UpdateResources();
        chooseBuilding();
        chooseUnit();

        timerTurno = 0f;

        unidadesAlly = new List<GameObject>(GameObject.FindGameObjectsWithTag("AllyUnit"));
        unidadesEnem =new List<GameObject> (GameObject.FindGameObjectsWithTag ("EnemyUnit"));
		if (defensivo) {
			foreach (GameObject unidad in unidadesEnem) {
				if (unidad.GetComponent<UnitClass> ().type == "raider") {
					unidad.GetComponent<UnitClass> ().Patrulla ();
				} else {
					unidad.GetComponent<UnitClass> ().Defensivo ();
				}
			}
		} else {
			foreach (GameObject unidad in unidadesEnem) {
				if (unidad.GetComponent<UnitClass> ().type == "raider") {
					unidad.GetComponent<UnitClass> ().Agresivo ();
				} else {
					unidad.GetComponent<UnitClass> ().Agresivo ();
				}
			}
		}

        MoveEnemyUnits();
    }


    void chooseUnit()
    {
        Unidad_deseada = Unidades.soldado;

        if (createTroops) {
            foreach(HexCell cell in recruitUnitBuildings)
            {
                bool finished = false;
                foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection))){
                    if (cell.GetNeighbor(direction).SpecialIndex == 0 && !cell.GetNeighbor(direction).Unit) {
                        if (haveResourcesUnits(Unidad_deseada))
                        {
                            createEnemy(cell.GetNeighbor(direction), Unidad_deseada);
                            finished = true;
                            break;
                        }
                    }
                }
                if (finished) {
                    break;
                }
            }
        }
    }

    void createEnemy(HexCell cell, Unidades unidad) {
        switch(unidad)
        {
            case Unidades.soldado:
                madera -= 20;    piedra -= 20;  comida -= 40;   aldeanos -= 1;
                Unidad_deseada = Unidades.arquero;
                hexGrid.AddUnit(Instantiate(enemyUnitPrefab[0]), cell, UnityEngine.Random.Range(0f, 360f),2);
                break;
            case Unidades.arquero:
                madera -= 45; piedra -= 25; comida -= 40; aldeanos -= 1;
                hexGrid.AddUnit(Instantiate(enemyUnitPrefab[1]), cell, UnityEngine.Random.Range(0f, 360f),2);
                Unidad_deseada = Unidades.jinete;
                break;
            case Unidades.jinete:
                madera -= 25; piedra -= 45; comida -= 50; aldeanos -= 1;
                hexGrid.AddUnit(Instantiate(enemyUnitPrefab[2]), cell, UnityEngine.Random.Range(0f, 360f),2);
                break;
            default:
                break;

        }
        tropas++;
        createTroops = false;
        

    }

    void MoveEnemyUnits()
    {
        foreach (GameObject enemy in unidadesEnem)
        {
            if (unidadesEnem.Count > unidadesAlly.Count)
            {
                defensivo = false;
                PathIAOfensiva(enemy);
            }
            else
            {
                //SE QUEDA EN ESTADO DEFENSIVO ALREDEDOR DEL CASTILLO
                defensivo = true;
            }
        }
    }

    void PathIAOfensiva(GameObject enemy)
    {
        //ELEGIMOS CASTILLO A ATACAR
        castillosAlly = GameObject.Find("Hex Feauture").GetComponent<HexFeatureManager>().GetCastillos();

        if(castillosAlly != null)
        {
            castilloPosible = new List<HexCell>();
            foreach (HexCell cell in castillosAlly)
            {
                if (cell!= null)
                {
                    enemyUnit = enemy.GetComponent<HexUnit>();
                    currentCell = enemyUnit.Location;
                    pos = cell.Position;
                    targetCell = hexGrid.GetCell(pos);
                    if (hexGrid.GetPath() == null)
                    {
                        hexGrid.FindPathNoShow(currentCell, targetCell, enemyUnit);
                        listGrid = hexGrid.GetPath();
                        listas.Add(listGrid);
                        castilloPosible.Add(targetCell);
                        // enemyUnit.Travel(listGrid);
                    }
                }

            }
            
            int min = 0;

            List<HexCell> elegido = listas[0];
            HexCell castilloElegido = castilloPosible[0];

            for (int i = 1; i < listas.Count; i++)
            {
                if(listas[i].Count < min)
                {
                    min = listas[i].Count;
                    elegido = listas[i];
                    castilloElegido = castilloPosible[i];
                }
            }

            //CALCULAMOS DEFINITIVO

            //UN MODO: NO SE QUE HARÁN LOS PATHS
            //enemyUnit.Travel(elegido);

            //OTRO MODO: DIBUJAR DE NUEVO EL PATH

            pos = castilloElegido.Position;
            targetCell = hexGrid.GetCell(pos);

            PathIA(enemyUnit, currentCell, targetCell, pos);

        }
    }

    void PathIA(HexUnit enemyUnit, HexCell currentCell, HexCell targetCell, Vector3 pos)
    {

        if (hexGrid.GetPath() == null)
        {
            hexGrid.FindPath(currentCell, targetCell, enemyUnit);
            listGrid = hexGrid.GetPath();
            enemyUnit.Travel(listGrid);
        }
    }

    /*void MoveEnemyUnits()
    {
        //Debug.Log("CANTIDAD");
        // Debug.Log(unidadesEnem.Count);

        foreach (GameObject enemy in unidadesEnem)
         {
             Debug.Log("ENEMYY");
             if (enemy != null)
             {
                //CALCULO VALORES
                enemyUnit = enemy.GetComponent<HexUnit>();
                currentCell = enemyUnit.Location;
                pos = new Vector3(69, 0, 120);
                targetCell = hexGrid.GetCell(pos);

                if (alAtaque && edificio >= 12)
                {
                    //A POR TODOS
                    PathIA(enemyUnit, currentCell, targetCell, pos);
                }
                else
                {
                    //vuelve a casa
                     if (currentCell == targetCell)
                     {
                         vuelve = true;
                     }
                     if (vuelve)
                     {
                        //CALCULO NUEVO VALOR
                        iniPos = new Vector3(155, 0, 180);
                        targetCell = hexGrid.GetCell(iniPos);

                        //VUELVE A CASA
                        PathIA(enemyUnit, currentCell, targetCell, pos);

                        //sal a por todos
                        if (currentCell == targetCell)
                        {
                             alAtaque = true;
                        }
                     }
                     else
                     {
                        //VE A MIRAR AL PLAYER
                        PathIA(enemyUnit, currentCell, targetCell, pos);
                     }

                }

             }

         }
    }*/



    bool haveResourcesBuilding(Edificio name)
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


    bool haveResourcesUnits(Unidades unit) {
        switch (unit)
        {
            case Unidades.soldado:
                if (madera > 20 && piedra > 20 && comida> 40 && aldeanos >= 1)
            {
                return true;
            }
            break;
            case Unidades.arquero:
                if (madera > 45 && piedra > 25 && comida > 40 && aldeanos >= 1)
            {
                return true;
            }
            break;
            case Unidades.jinete:
                if (madera > 25 && piedra > 45 && comida > 50 && aldeanos >= 1)
            {
                return true;
            }
            break;
        }
        return false;
    }


    void build(Edificio name)
    {
        foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
        {
            int index = 0;
            HexCell cell = castillos[castillo - 1].GetNeighbor(direction);
            if (cell.SpecialIndex == 0)
            {
                switch (name)
                {
                    case Edificio.aserradero:
                        piedra -= 50;
                        aldeanos--;
                        index = 2;
                        recursos_turno[0]++;
                        break;
                    case Edificio.mina:
                        madera -= 50;
                        aldeanos--;
                        index = 3;
                        recursos_turno[1]++;
                        break;
                    case Edificio.molino:
                        piedra -= 25;
                        madera -= 25;
                        aldeanos--;
                        index = 4;
                        recursos_turno[2]++;
                        break;
                    case Edificio.cuartel:
                        piedra -= 35;
                        madera -= 35;
                        index = 5;
                        offensive_building[0]++;
                        aldeanos--;
                        recruitUnitBuildings.Add(cell);
                        break;
                    case Edificio.arqueria:
                        piedra -= 35;
                        madera -= 35;
                        index = 6;
                        offensive_building[1]++;
                        recruitUnitBuildings.Add(cell);
                        aldeanos--;
                        break;
                    case Edificio.caballeria:
                        piedra -= 35;
                        madera -= 35;
                        index = 7;
                        offensive_building[2]++;
                        recruitUnitBuildings.Add(cell);
                        aldeanos--;
                        break;
                    default:
                        break;
                }
                cell.SpecialIndex = index;
                StartCoroutine(BuildBuilding(index, cell));
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
            castillos[castillo].GetNeighbor(direction).Walled = true;
			castillos[castillo].GetNeighbor(direction).Owner = 2;
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
            if (edificio > 9 && tropas <= edificio/3)
            {
                createTroops = true;

            }

            if (edificio == 6) {
                tower[0].SpecialIndex = 10;
                tower[0].Owner = 2;
            }
            if (edificio == 10)
            {
                tower[1].SpecialIndex = 9;
                tower[0].Owner = 2;
				tower [1].Owner = 2;
            }
            if ((castillo < 1 || edificio >= castillo * 6) && haveResourcesBuilding(Edificio.castillo))
            {
                buildCastillo(castillo);
            }
            else
            {                
                if ((recursos_turno[2] == 0 || recursos_turno[2] <= castillo - 1) && haveResourcesBuilding(Edificio.molino))
                {
                    build(Edificio.molino);
                }
                else if ((recursos_turno[0] == 0 || recursos_turno[0] <= castillo - 1) && haveResourcesBuilding(Edificio.aserradero))
                {
                    build(Edificio.aserradero);
                }
                else if ((recursos_turno[1] == 0 || recursos_turno[1] <= castillo - 1) && haveResourcesBuilding(Edificio.mina))
                {
                    build(Edificio.mina);
                }
                if (recursos_turno.Min() > 0 && construido[0])
                {
                    tower[0].SpecialIndex = 8;
                    construido[0] = true;
                }

                else if ((offensive_building[0] == 0 || offensive_building[0] <= castillo - 1) && haveResourcesBuilding(Edificio.cuartel))
                {
                    build(Edificio.cuartel);
                }
                else if ((offensive_building[1] == 0 || offensive_building[1] <= castillo - 1) && haveResourcesBuilding(Edificio.caballeria))
                {
                    build(Edificio.caballeria);
                }
                else if ((offensive_building[2] == 0 || offensive_building[2] <= castillo - 1) && haveResourcesBuilding(Edificio.arqueria))
                {
                    build(Edificio.arqueria);
                }
                
                if (offensive_building[0] != 0 && recursos_turno[1] != 0 && tropas <= edificio / 3)
                {
                    createTroops = true;
                }
            }
        }
    }



    public IEnumerator BuildBuilding(int index, HexCell cell)
    {
        //Debug.Log("ENTRANDO CORRUTINA");
        yield return new WaitForSeconds(4);
        //     cell.SpecialIndex = ++index;
        //Debug.Log("index -> " + cell.SpecialIndex);
        //Debug.Log("APLICADO EL CAMBIO");

    }

}




