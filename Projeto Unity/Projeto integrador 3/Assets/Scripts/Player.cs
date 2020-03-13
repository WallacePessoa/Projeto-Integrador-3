using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float PowerUpSpeed;
    public float SpeedLocal;
    public float SpeedRotation;

    public GameObject Camera;

    Rigidbody rb;

    float hori;
    float vert;


    Vector3 MousePosition;

    Ray ray;

    RaycastHit hit;


    int auxWhile = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SpeedLocal = Speed;

    }

    // Update is called once per frame
    void Update()
    {
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");


        ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            rb.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, hit.point.z * -1), Color.red, 1);

        }

        ///rb.AddForce(PontoFixo.transform.forward * vert * Speed);
        //rb.AddForce(PontoFixo.transform.right * hori * Speed);

        //rb.velocity = new Vector3(-hori,0, -vert) * SpeedLocal;

        rb.velocity = transform.forward * vert * SpeedLocal;

        //rb.transform.Rotate(new Vector3(0, hori, 0) * SpeedRotation);

    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {

            if(SpeedLocal > 0)
            {
                SpeedLocal -= 1;
                yield return new WaitForSeconds(3f);

                SpeedLocal += 1;
            }

        }
        if (other.CompareTag("PowerUpSpeed"))
        {

            SpeedLocal = PowerUpSpeed;
            while(SpeedLocal > Speed)
            {
                //rb.velocity = new Vector3(-hori, 0, -vert) * SpeedLocal;
                rb.velocity = transform.forward * vert * SpeedLocal;
                SpeedLocal -= 1;
                yield return new WaitForSeconds(0.01f);

                print("aaa");



                if (auxWhile > 100)
                {
                    break;
                }
                auxWhile++;
            }



            print("aaa");
        }


    }
}
