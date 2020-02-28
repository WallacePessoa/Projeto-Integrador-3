using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public float Speed;
    public GameObject PontoFixo;

    Rigidbody rb;

    float hori;
    float vert;
    public float SpeedLocal;



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SpeedLocal = Speed;
    }

    // Update is called once per frame
    void Update()
    {

        rb.AddTorque(PontoFixo.transform.right * SpeedLocal);
        //rb.AddForce(PontoFixo.transform.forward * SpeedLocal);


    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {

            if(rb.velocity.magnitude > 0)
                SpeedLocal -= 0.1f;

        }

        yield return new WaitForSeconds(3f);
        if(SpeedLocal != Speed)
            SpeedLocal += 0.1f;

    }
}
