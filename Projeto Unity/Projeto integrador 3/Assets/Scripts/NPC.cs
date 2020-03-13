using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Camera Camera;

    public float Speed;
    public float SpeedLocal;
    public float SpeedFire;

    public Vector3 PosDestino;
    public Vector3[] Posiçoes;

    public GameObject PontoFixo;
    public GameObject[] Positions;

    public GameObject Fire;
    public GameObject GunL;
    public GameObject GunR;

    GameObject fireLocal;

    Rigidbody FireRb;


    float hori;
    float vert;
    int Destino = 0;

    NavMeshAgent Nav;

    Ray ray;

    RaycastHit hit;


    public enum StateMachine
    {
        Correr,
        Curva,
        Atirar
    }

    public StateMachine Estado;

    void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        Estado = StateMachine.Correr;
        SpeedLocal = Speed;

        PosDestino = new Vector3(Positions[Destino].transform.position.x + Random.Range(-10, 10), Positions[Destino].transform.position.y, Positions[Destino].transform.position.z + Random.Range(-10, 10));

        for (int x = 0; x< Positions.Length;x++)
            Posiçoes[x] = Positions[x].transform.position;

        
    }

    // Update is called once per frame
    void Update()
    {

        switch (Estado)
        {
            case StateMachine.Correr:


                Nav.destination = PosDestino;

                if (Vector3.Distance(transform.position, PosDestino) < 5)
                {
                    Estado = StateMachine.Curva;
                }
                break;

            case StateMachine.Curva:



                if (Destino+1 < Positions.Length)
                {
                    Destino++;
                }

                PosDestino = new Vector3(Positions[Destino].transform.position.x + Random.Range(-10, 10), Positions[Destino].transform.position.y, Positions[Destino].transform.position.z + Random.Range(-10, 10));
                Estado = StateMachine.Correr;

                break;

            case StateMachine.Atirar:

                Nav.destination = PosDestino;

                fireLocal = Instantiate(Fire, GunL.transform.position, transform.rotation);

                FireRb = fireLocal.GetComponent<Rigidbody>();

                FireRb.velocity = transform.forward * SpeedFire;

                fireLocal = Instantiate(Fire, GunR.transform.position, transform.rotation);

                FireRb = fireLocal.GetComponent<Rigidbody>();

                FireRb.velocity = transform.forward * SpeedFire;

                if (Vector3.Distance(transform.position, PosDestino) < 5)
                {
                    Estado = StateMachine.Curva;
                }

                break;
        }   

    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {

            //if(SpeedLocal > 0)
            //{

            //}
            //    SpeedLocal -= 1f;

            if (Nav.speed > 0)
            {
                SpeedLocal -= 1;
                Nav.speed -= 1;
                yield return new WaitForSeconds(3f);
                Nav.speed += 1;
                SpeedLocal += 1;
            }

        }

        //yield return new WaitForSeconds(3f);
        //if(SpeedLocal != Speed)
        //    SpeedLocal += 0.1f;

    }
}
