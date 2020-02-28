using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookArmNPC : MonoBehaviour
{


    Rigidbody rb;
    public GameObject PontoFixo;


    void Start()
    {
        rb = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.LookAt(PontoFixo.transform);

        transform.localPosition = Vector3.zero;




    }
}
