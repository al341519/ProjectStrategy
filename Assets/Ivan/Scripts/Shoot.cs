using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public Transform target;

    GameObject arrow;
    Transform pos;
    bool shoot;
    bool destroy;

    float shootAngle = 30;
    Vector3 dir;
    float vel;
    float h;
    float dist;
    float a;

    float velocityY;
    float velocityX;
    float velocityZ;
    Rigidbody rb;

    public const int maxHealth = 100;
    public int currentHealth = maxHealth;

    // Use this for initialization
    void Start () {
        destroy = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("b") && destroy)
        {  // press b to shoot
            shoot = true;
            destroy = false;
            Fire();
        }

        if (shoot)
        {
            rotateObject();
        }

    }

    void Fire() {

        /*var arrow = (GameObject)Instantiate( arrowPrefab,  arrowSpawn.position,  arrowSpawn.rotation);
        
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * 6;
        Destroy(arrow, 2.0f);*/

        arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
        pos = arrow.transform;
        pos.LookAt(target.position);

        rb = arrow.GetComponent<Rigidbody>();

        rb.velocity = BalisticVel(target, shootAngle);
        velocityY = rb.velocity.y;
        rb.AddForce(Vector3.forward * velocityY * Time.deltaTime * 2);

        Destroy(arrow, 10);
        shoot = false;
        destroy = true;
    }

    void rotateObject()
    {
        velocityX = rb.velocity.x;
        velocityZ = rb.velocity.z;
        float combVelocity = Mathf.Sqrt(velocityX * velocityX + velocityZ * velocityZ);
        float fallAngle = -1 * Mathf.Atan2(velocityY, combVelocity) * 180 / Mathf.PI;
        pos.eulerAngles = new Vector3(fallAngle, pos.eulerAngles.y, pos.eulerAngles.z);
    }

    Vector3 BalisticVel(Transform target, float angle)
    {
        dir = target.position - transform.position;
        h = dir.y;
        dir.y = 0;
        dist = dir.magnitude;
        a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        dist += h / Mathf.Tan(a);
        vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
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
