using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Camera Camera;
    public float Speed;
    public GameObject PontoFixo;

    Rigidbody rb;

    float hori;
    float vert;
    public float SpeedLocal;


    Ray ray;

    RaycastHit hit;



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SpeedLocal = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Physics.Raycast(transform.position,transform.forward * 20, out hit, Mathf.Infinity))
        {
            rb.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, hit.point.z), Color.blue, 1);

        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            rb.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, hit.point.z), Color.yellow, 1);

        }

        if (Physics.Raycast(transform.position, transform.forward * -20, out hit, Mathf.Infinity))
        {
            rb.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, hit.point.z), Color.green, 1);

        }

        rb.velocity = transform.forward * SpeedLocal;
        //rb.AddTorque(transform.forward * SpeedLocal);
        //rb.AddForce(PontoFixo.transform.forward * SpeedLocal);


    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {

            if(SpeedLocal > 0)
                SpeedLocal -= 0.1f;

        }

        yield return new WaitForSeconds(3f);
        if(SpeedLocal != Speed)
            SpeedLocal += 0.1f;

    }
}
