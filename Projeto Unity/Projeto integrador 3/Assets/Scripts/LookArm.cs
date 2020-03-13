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

        //ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    rb.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        //    Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, hit.point.z * -1), Color.red, 1);

        //}

        //transform.localPosition = Vector3.zero;

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
