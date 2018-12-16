using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    Transform target; 

    //TARGET PRIVATE IF CELL FUNCIONA
    /* Transform target;
     * HexCell cell;
     * HexUnit unit;
     * HexEnemyUnit enemy;*/

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

    HexCell cell;
    int countEnemy;

    // Use this for initialization
    void Start()
    {
        destroy = true;
        countEnemy = 0;
        cell = gameObject.GetComponent<Information>().GetCell();
    }

    // Update is called once per frame
    void Update()
    {


        HexCell[] vecinos = cell.GetVecinos();
        for (int i = 0; i < vecinos.Length; i++) //RANGO 1
        { 
            HexCell[] vecinos2 = vecinos[i].GetVecinos();

            for (int k = 0; i < vecinos2.Length; k++) //RANGO 2
            {
                HexCell[] vecinos3 = vecinos2[k].GetVecinos();

                for (int j = 0; j < vecinos3.Length; j++) //RANGO 3
                {
                    if (countEnemy == 3)
                    {
                        break;
                    }

                    if (vecinos3[j].Unit != null) //COMPRUEBA LOS ENEMIGOS DE RANGO 3 Y 1
                    {
                        HexUnit unit = vecinos3[j].Unit;
                        if (cell.Owner == 1 && unit.tag == "EnemyUnit")
                        {
                            target = unit.transform;
                            shoot = true;
                            Fire();
                            countEnemy++;
                        }
                        else if (cell.Owner == 2 && unit.tag == "AllyUnit")
                        {
                            target = unit.transform;
                            shoot = true;
                            Fire();
                            countEnemy++;
                        }

                    }
                }

                if (countEnemy == 3)
                {
                    countEnemy = 0;
                    break;
                }

                if (vecinos2[i].Unit != null) //COMPRUEBA LOS ENEMIGOS DE RANGO 2
                {
                    HexUnit unit = vecinos2[i].Unit;
                    if (cell.Owner == 1 && unit.tag == "EnemyUnit")
                    {
                        target = unit.transform;
                        shoot = true;
                        Fire();
                        countEnemy++;
                    }
                    else if (cell.Owner == 2 && unit.tag == "AllyUnit")
                    {
                        target = unit.transform;
                        shoot = true;
                        Fire();
                        countEnemy++;
                    }

                }

            }
        }

        /*if (Input.GetKeyDown("b") && destroy)
        {  // press b to shoot
            shoot = true;
            destroy = false;
            Fire();
        }*/

        if (shoot)
        {
            rotateObject();
        }

    }

    void Fire()
    {
        arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
        pos = arrow.transform;
        pos.LookAt(target.position);

        rb = arrow.GetComponent<Rigidbody>();

        rb.velocity = BalisticVel(target, shootAngle);
        velocityY = rb.velocity.y;
        rb.AddForce(Vector3.forward * velocityY * Time.deltaTime * 2);

        Destroy(arrow, 2);
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
