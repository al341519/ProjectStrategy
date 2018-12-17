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
    

    HexCell cell;
    int countEnemy;

    // Use this for initialization
    void Start()
    {
        destroy = true;
        shoot = true;
        countEnemy = 0;
        cell = gameObject.GetComponent<Health>().GetCelda();
    }

    // Update is called once per frame
    void Update()
    {
        if (cell != null)
        {
            HexCell[] vecinos = cell.GetVecinos();
            Debug.Log("EMPIEZA BUCLE 1");

            for (int i = 0; i < vecinos.Length; i++) //RANGO 1
            {
                Debug.Log(i);
                HexCell[] vecinos2 = vecinos[i].GetVecinos();

                Debug.Log("EMPIEZA BUCLE 2");

                for (int k = 0; k < vecinos2.Length; k++) //RANGO 2
                {
                    Debug.Log(k);
                    HexCell[] vecinos3 = vecinos2[k].GetVecinos();

                    Debug.Log("EMPIEZA BUCLE 3");

                    for (int j = 0; j < vecinos3.Length; j++) //RANGO 3
                    {
                        if (countEnemy == 3)
                        {
                            break;
                        }

                       /* //ERROR WHY?
                        Debug.Log(j);
                        Debug.Log(vecinos3);
                        
                        //Debug.Log(vecinos3[j]);
                        Debug.Log(vecinos3[j].Unit);*/

                        if (vecinos3[j] != null && vecinos3[j].Unit != null) //COMPRUEBA LOS ENEMIGOS DE RANGO 3 Y 1
                        {
                            HexUnit unit = vecinos3[j].Unit;

                            //PRUEBA
                            target = unit.transform;

                            

                            if (shoot && countEnemy < 3)
                            {
                                shoot = false;
                                Debug.Log("DISPARA YA");
                                Fire();
                                StartCoroutine(WaitForArrow());
                                countEnemy++;
                            }
                           

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

                    if (vecinos2[k].Unit != null) //COMPRUEBA LOS ENEMIGOS DE RANGO 2
                    {
                        HexUnit unit = vecinos2[k].Unit;
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
        StartCoroutine(WaitForArrow());
        //shoot = false;
        destroy = true;
    }

    IEnumerator WaitForArrow()
    {

        yield return new WaitForSeconds(2);
        shoot = true;
    }

    void rotateObject()
    {
        /*velocityX = rb.velocity.x;
        velocityZ = rb.velocity.z;
        float combVelocity = Mathf.Sqrt(velocityX * velocityX + velocityZ * velocityZ);
        float fallAngle = -1 * Mathf.Atan2(velocityY, combVelocity) * 180 / Mathf.PI;
        pos.eulerAngles = new Vector3(fallAngle, pos.eulerAngles.y, pos.eulerAngles.z);*/

        pos.forward = rb.velocity.normalized;

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

    
}
