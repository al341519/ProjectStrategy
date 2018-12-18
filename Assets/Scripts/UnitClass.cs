using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClass : MonoBehaviour {

	[HideInInspector]public bool combat;
	private HexUnit target=null;
	private UnitClass targetClass;
	private Health targetEdi;
	private float cdAtack;

	private HexGrid grid;
	private List<HexCell> listGrid;
	public HexGameUI uiGame;
	private List<GameObject> unidadesEnemigas;
	private float radiocelda = HexMetrics.innerRadius * 2;

    Animator anim;//Animaciones
   

    public bool caminar;
    public bool atacar;

	[Header("Unit Type")]
	public string type="infantry";//infantry,raider,archer,villager

	[Header("Stats")]
	public float velocity;
	public float defence;
	public float attack;
	public float life;
	public float visionRange;
	public float attackRange;

	[Header("Comportamientos")]
	public bool agresivo=false;
	public bool huidizo=false;
	public bool patrulla=false;

	private bool caminoHuida=true;
	public bool agresivoMov=false;
	public bool cdPatrulla = true;

	private float rangoAgresividad;
	private bool codigoPlayer;

	private HexCell ultimaPatrulla=null;

    int index;


    // Use this for initialization
    void Start () {
		uiGame = GameObject.Find("Game UI").GetComponent<HexGameUI>();
		grid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
		cdPatrulla = true;
        if (type != "infantry") {
            anim = this.GetComponent<Animator>();
        }
        if (this.GetComponent<HexUnit>().propietario == 1) { this.tag = "AllyUnit"; }
        else { this.tag = "EnemyUnit"; }
        if (tag == "AllyUnit")
        {
            GetComponent<Influencer>()._Team = 1;
        }
        if(tag == "EnemyUnit")
        {
            GetComponent<Influencer>()._Team = 2;
        }

        caminar = false;
        atacar = false;

		switch(type){
		case "infantry":
			//susvalores
			velocity = 3f;
			defence = 2f;
			attack = 1f;
			life = 1f;
			visionRange = 2f;
			attackRange = 1f;
			break;
		case "raider":
			velocity = 3f;
			defence = 1f;
			attack = 1f;
			life = 1f;
			visionRange = 3f;
			attackRange = 1f;
			break;
		case "archer":
			velocity = 2f;
			defence = 0.5f;
			attack = 1.5f;
			life = 1f;
			visionRange = 3f;
			attackRange = 2f;
			break;
		case "villager":
			velocity = 1f;
			defence = 1f;
			attack = 0f;
			life = 1f;
			visionRange = 2f;
			attackRange = 1f;
			break;
		default:
			break;
				
		}

    }
	
	// Update is called once per frame
	void Update () {
		if (life<=0){
			Death();
		}
		
		if (agresivo) {
			//Coger enemigos a X distancia e ir a por ellos
			if (this.tag == "AllyUnit") {
				unidadesEnemigas= new List<GameObject> (GameObject.FindGameObjectsWithTag ("EnemyUnit"));
			} else if (this.tag == "EnemyUnit") {
				unidadesEnemigas = new List<GameObject> (GameObject.FindGameObjectsWithTag ("AllyUnit"));
			}
			if (agresivoMov) {
				rangoAgresividad = attackRange;
			} else {
				rangoAgresividad = visionRange;
			}
			foreach (GameObject unidad in unidadesEnemigas) {
				float distancia= Mathf.Abs (Vector3.Distance (this.transform.position, unidad.transform.position));
					if (distancia <= 0.5+rangoAgresividad * radiocelda && !target) {
							target = unidad.GetComponent<HexUnit> ();
							HexUnit thisUnit = this.GetComponent<HexUnit> ();
							//thisUnit;
							grid.ClearPath ();
							grid.FindPath (thisUnit.Location, target.Location, thisUnit);
							listGrid = grid.GetPath ();
							thisUnit.Travel (listGrid);
							break;
					}
			}
		}

		if (huidizo) {
			//Al ver un enemigo a X distancia retroceder

			if (this.tag == "AllyUnit") {
				unidadesEnemigas= new List<GameObject> (GameObject.FindGameObjectsWithTag ("EnemyUnit"));
			} else if (this.tag == "EnemyUnit") {
				unidadesEnemigas = new List<GameObject> (GameObject.FindGameObjectsWithTag ("AllyUnit"));
			}

			foreach (GameObject unidad in unidadesEnemigas) {
				float distancia = Mathf.Abs (Vector3.Distance (this.transform.position, unidad.transform.position));

				if (distancia <= visionRange  * radiocelda) {
					//Debug.Log ("Estoy mirando el rango");
					HexCell[] listaVecinos = this.GetComponent<HexUnit> ().Location.GetVecinos ();
					float distanciaEntreCeldas = -999999999;
					HexCell celdaAlejada = null;
					Debug.Log ("El array de celdas tiene: "+listaVecinos.Length);
					foreach (HexCell cell in listaVecinos) {//Encontrar la más alejada
						if (!cell.Unit) {
							float distanciaAComparar = Mathf.Abs (Vector3.Distance (cell.transform.position, unidad.transform.position));
							if (distanciaEntreCeldas <= distanciaAComparar) {
								celdaAlejada = cell;
								distanciaEntreCeldas = distanciaAComparar;
							}
						}
					}
					if (celdaAlejada && caminoHuida) {
						//Debug.Log ("Entro al bucle de huida");
						/*grid.ClearPath ();
						grid.FindPath (this.GetComponent<HexUnit> ().Location, celdaAlejada, this.GetComponent<HexUnit> ());
						listGrid = grid.GetPath ();
						this.GetComponent<HexUnit> ().Travel (listGrid);*/
						listGrid = new List<HexCell> ();
						listGrid.Add (this.GetComponent<HexUnit> ().Location);
						listGrid.Add (celdaAlejada);
						this.GetComponent<HexUnit> ().Travel (listGrid);
						caminoHuida = false;
						StartCoroutine ("TiempoHuida");
						break;
					}

				}

			}

		}

        if (patrulla)
        {
            
            HexCell[] listaCeldas = this.GetComponent<HexUnit>().Location.GetVecinos();
            foreach (HexCell celdaPatrulla in listaCeldas)
            {
                if(this.tag == "AllyUnit")
                {
                    index = 0;
                }
                else if (this.tag == "EnemyUnit")
                {
                    index = 1;
                }
               

                /*Debug.Log(celdaPatrulla);
                Debug.Log(celdaPatrulla.influenceInfo[index]);
                Debug.Log(celdaPatrulla.influenceInfo[index].IsBuildingFrontier);*/

               /* Debug.Log("FRONTERA");

                Debug.Log(celdaPatrulla);
                Debug.Log(celdaPatrulla.influenceInfo[index]);
                Debug.Log(celdaPatrulla.influenceInfo[index].IsBuildingFrontier);*/

                //GetFrontier();

                if (celdaPatrulla != null && celdaPatrulla.influenceInfo[index].IsBuildingFrontier && ultimaPatrulla != celdaPatrulla)
                {
                    Debug.Log("HA ENTRADO");
                    ultimaPatrulla = celdaPatrulla;
                    listGrid = new List<HexCell>();
                    listGrid.Add(this.GetComponent<HexUnit>().Location);
                    listGrid.Add(celdaPatrulla);
                    this.GetComponent<HexUnit>().Travel(listGrid);
                    cdPatrulla = false;
                    StartCoroutine("PatrullaCD");
                }
                
            }
        }

       
        if (cdAtack <= 0) {
			if (combat) {//Distancia entre objetivos hay que tenerla en cuenta
				if (target) {
					float distancia = Mathf.Abs (Vector3.Distance (this.transform.position, target.transform.position));
					if (distancia <= ((attackRange * radiocelda) + 0.1f)) {
						
						targetClass = target.GetComponent<UnitClass> ();
						targetClass.dealDMG (this.attack, this.GetComponent<HexUnit> ());
						cdAtack = HexMetrics.tiempo*3;
						atacar = true;
					} else {
						listGrid = new List<HexCell> ();
						listGrid.Add (this.GetComponent<HexUnit> ().Location);
						listGrid.Add (target.Location);
						this.GetComponent<HexUnit> ().Travel (listGrid);
						atacar = false;

					}
				} else if (targetEdi) {
					targetEdi.TakeDamage ((int)this.attack);
					cdAtack = HexMetrics.tiempo*3;
					atacar = true;
				}

			} else {
				atacar = false;
				target = null;
				targetEdi = null;
			}
		} else {
			
			cdAtack -= Time.deltaTime;
		}

        if (type != "infantry")
        {
            if (caminar)//Activar/desactivar animaciones
            {
                anim.SetBool("caminar", true);
                anim.SetBool("atacar", false);
            }
            else
            {
                anim.SetBool("caminar", false);
            }
            if (atacar)
            {
                anim.SetBool("atacar", true);
                anim.SetBool("caminar", false);
            }
            else
            {
                anim.SetBool("atacar", false);
            }
        }
    }

    public void GetFrontier()
    {
        HexCell[] celdas = grid.GetCells();

        foreach (HexCell celda in celdas)
        {
            if (celda != null && celda.influenceInfo[0].IsBuildingFrontier)
            {
                Debug.Log(celda.Position);
            }
        }
    }

    public void dealDMG(float damageRecive,HexUnit enemy){
		this.life -= (damageRecive / this.defence);

		if (!combat){
			HexCell[] vecinos = this.GetComponent<HexUnit> ().Location.GetVecinos ();
			if (this.attackRange == 1f) {
				foreach (HexCell unitCell in vecinos) {
					if (unitCell.Unit == enemy) {
						combat = true;
						target = enemy;
						cdAtack = HexMetrics.tiempo;
						break;
					}
				}
				if (!combat) {
					HexUnit thisUnit = this.GetComponent<HexUnit> ();
					//thisUnit;
					grid.ClearPath ();
					grid.FindPath (thisUnit.Location, enemy.Location, thisUnit);
					listGrid = grid.GetPath();
					thisUnit.Travel(listGrid);

				}
			} else {
				combat = true;
				target = enemy;
				cdAtack = HexMetrics.tiempo;
			}


		}
	}
	public void targetPut(HexUnit unit){
		//Debug.Log ("Mi objetivo es: "+unit.name);
		target = unit;
		combat = true;
	}
	public void targetPut(Health edif){
		targetEdi = edif;
		combat = true;
	}

	private void Death(){
		this.GetComponent<HexUnit> ().Die ();
	}

	public void Agresivo(){
		agresivo = true;
		huidizo = false;
		patrulla = false;
	}
	public void Huidizo(){
		agresivo = false;
		huidizo = true;
		patrulla = false;
	}
	public void Patrulla(){
		agresivo = false;//puede que rente ponerlo a true
		huidizo = false;
		patrulla = true;
	}
	public void Defensivo(){
		agresivo = false;
		huidizo = false;
		patrulla = false;
	}

	private IEnumerator TiempoHuida(){
		yield return new WaitForSeconds (1);
		caminoHuida = true;
	}

	private IEnumerator PatrullaCD(){
		yield return new WaitForSeconds (2);
		cdPatrulla = true;
	}
}
