using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float PowerUpSpeed;
    public GameObject PontoFixo;

    Rigidbody rb;

    float hori;
    float vert;
    public float SpeedLocal;


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


        ///rb.AddForce(PontoFixo.transform.forward * vert * Speed);
        //rb.AddForce(PontoFixo.transform.right * hori * Speed);

        rb.velocity = new Vector3(-hori,0, -vert) * SpeedLocal;

    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            SpeedLocal -= 1;
            yield return new WaitForSeconds(3f);
            SpeedLocal += 1;
        }
        if (other.CompareTag("PowerUpSpeed"))
        {

            SpeedLocal = PowerUpSpeed;
            while(SpeedLocal > Speed)
            {
                rb.velocity = new Vector3(-hori, 0, -vert) * SpeedLocal;
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
