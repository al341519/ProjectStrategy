using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public const int maxHealth = 100;
    public int currentHealth = maxHealth;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


       // Fire();


        /*if ((target.transform.positon - transform.position).sqrMagnitude < someDistanceSquared)
            Detect = true;
        if (Detect)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }*/
    }

    void Fire() {

        var arrow = (GameObject)Instantiate( arrowPrefab,  arrowSpawn.position,  arrowSpawn.rotation);
        
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * 6;
        Destroy(arrow, 2.0f);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
            //UPDATE EN EL MAPA INFLUENCIA
            Destroy(this, 0.5f);
        }
    }
}
