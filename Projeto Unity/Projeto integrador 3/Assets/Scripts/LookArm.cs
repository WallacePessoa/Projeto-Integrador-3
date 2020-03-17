using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookArm : MonoBehaviour
{
    public GameObject Payer;
    public Camera Camera;
    public GameObject Fire;
    public GameObject GunL;
    public GameObject GunR;

    public float SpeedFire;

    Rigidbody FireRb;

    GameObject fireLocal;

    Vector3 MousePosition;

    Rigidbody rb;

    Ray ray;

    RaycastHit hit;





    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {



        if(Input.GetKey(KeyCode.Mouse0)){

            fireLocal = Instantiate(Fire, GunL.transform.position, transform.rotation);

            FireRb = fireLocal.GetComponent<Rigidbody>();

            FireRb.velocity = transform.forward * SpeedFire;

            fireLocal = Instantiate(Fire, GunR.transform.position, transform.rotation);

            FireRb = fireLocal.GetComponent<Rigidbody>();

            FireRb.velocity = transform.forward * SpeedFire;

        }

    }
}
