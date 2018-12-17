using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        //SI TUVIERAN COLLIDER
        /* var hit = col.gameObject;
         var vida = hit.GetComponent<UnitClass>();

         if (vida != null)
         {
             if (hit.tag == "EnemyUnit")
             {
                 vida.life -= 0.5f;
             }

             else if (col.gameObject.tag == "AllyUnit")
             {
                 vida.life -= 0.5f;
             }
         }*/
         
    }
}
