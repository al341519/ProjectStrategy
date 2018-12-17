using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testShoot : MonoBehaviour {

    public GameObject arrowPrefab;
    GameObject arrow;
    Rigidbody arrowRG;

    public Transform shootOrigin;

    public Transform shootTarget;

    public float _TimeBetweenShoots;
    public float _Height;

    bool isShot = true;

    void Awake()
    {
        //arrowRG = arrow.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isShot)
        {
            StartCoroutine("ShootArrow");
        }

    }

    IEnumerator ShootArrow()
    {
        isShot = false;
        arrow = Instantiate(arrowPrefab, shootOrigin.position, transform.rotation);
        arrow.transform.rotation *= Quaternion.Euler(0,90,0);
        arrowRG = arrow.GetComponent<Rigidbody>();
        Vector3 distance = shootTarget.position - arrow.transform.position;
        Vector3 velocity = distance / _TimeBetweenShoots;
        arrowRG.velocity = velocity;

        float verticalImpulse = _Height-(0.5f*Physics.gravity.y*Mathf.Pow(_TimeBetweenShoots/2,2));
        arrowRG.velocity += new Vector3(0,verticalImpulse,0);
        //arrowRG.AddForce(new Vector3(0, verticalImpulse, 0), ForceMode.Force);

        yield return new WaitForSeconds(_TimeBetweenShoots);
        isShot = true;
        Destroy(arrow);
    }

    void Rotate()
    {

    }

}
