using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClass : MonoBehaviour {

	private bool combat;
	private HexUnit target=null;
	private UnitClass targetClass;
	private Health targetEdi;
	private float cdAtack;
	[Header("Unit Type")]
	public string type="infantryHOLA";//infantry,raider,archer,villager

	[Header("Stats")]
	public float velocity;
	public float defence;
	public float attack;
	public float life;
	public float visionRange;
	public float attackRange;



	// Use this for initialization
	void Awake () {
		switch(type){
		case "infantry":
			//susvalores
			velocity = 1f;
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
		if (cdAtack <= 0) {
			if (combat) {
				if (target) {
					targetClass = target.GetComponent<UnitClass> ();
					targetClass.dealDMG (this.attack, this.GetComponent<HexUnit> ());
					cdAtack = HexMetrics.tiempo;
				}
				else if(targetEdi){
					targetEdi.TakeDamage ((int)this.attack);
					cdAtack = HexMetrics.tiempo;
				}

			}
		} else {
			cdAtack -= Time.deltaTime;
		}
	}

	public void dealDMG(float damageRecive,HexUnit enemy){
		this.life -= (damageRecive / this.defence);

		if (!combat){
			combat = true;
			target = enemy;
			cdAtack = HexMetrics.tiempo;
		}
	}
	public void targetPut(HexUnit unit){
		Debug.Log ("Mi objetivo es: "+unit.name);
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
}
