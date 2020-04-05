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


    public GameObject PontoFixo;
    public GameObject[] Positions;

    public GameObject FirePadrao;
    public GameObject PowerFire;
    public GameObject GunL;
    public GameObject GunR;

    GameObject fireLocal;
    GameObject FireStandart;

    //Vector3[] Posiçoes;

    Rigidbody FireRb;


    float hori;
    float vert;
    public float Aceleração = 0;
    int Destino = 0;
    int Sortear;


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
        StartCoroutine(Acelerar());
        Nav = GetComponent<NavMeshAgent>();
        Estado = StateMachine.Correr;
       
        Positions = GameObject.FindGameObjectsWithTag("Nodes");


        PosDestino = new Vector3(Positions[Destino].transform.position.x + Random.Range(-10, 10), Positions[Destino].transform.position.y, Positions[Destino].transform.position.z + Random.Range(-10, 10));

        //for (int x = 0; x< Positions.Length;x++)
        //    Posiçoes[x] = Positions[x].transform.position;

        FireStandart = FirePadrao;



    }

    // Update is called once per frame
    void Update()
    {
        

        Nav.speed = Aceleração;
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

                fireLocal = Instantiate(FireStandart, GunL.transform.position, transform.rotation);

                FireRb = fireLocal.GetComponent<Rigidbody>();

                FireRb.velocity = transform.forward * SpeedFire;

                fireLocal = Instantiate(FireStandart, GunR.transform.position, transform.rotation);

                FireRb = fireLocal.GetComponent<Rigidbody>();

                FireRb.velocity = transform.forward * SpeedFire;

                if (Vector3.Distance(transform.position, PosDestino) < 5)
                {
                    Estado = StateMachine.Curva;
                }

                break;
        }   

    }

    private IEnumerator Acelerar()
    {
        if (Aceleração < Speed)
        {
            Aceleração += 1f;

            yield return new WaitForSeconds(0.05f);

        }
        else
        {
            Nav.speed = Speed;
        }



        yield return null;
        StartCoroutine(Acelerar());
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {

            if (Nav.speed > 19)
            {
                Nav.speed -= 0.1f;
                yield return new WaitForSeconds(2f);
                Nav.speed += 0.1f;
            }
            if(other.gameObject != null)
            {
                Destroy(other.gameObject);
            }



        }

        if (other.CompareTag("PowerLaser"))
        {

            if (Nav.speed > 9)
            {
                Nav.speed -= 0.1f;
                yield return new WaitForSeconds(2f);
                Nav.speed += 0.1f;
            }

            Destroy(other.gameObject);

        }

        if (other.CompareTag("PowerUp"))
        {
            Sortear = Random.Range(1, 5);

            switch (Sortear)
            {
                case 1:

                    FireStandart = PowerFire;

                    yield return new WaitForSeconds(3f);

                    FireStandart = FirePadrao;

                    break;

                case 2:

                    while(Nav.speed > 15)
                    {
                        Nav.speed -= 0.1f;
                        yield return new WaitForSeconds(0.1f);
                    }

                    yield return new WaitForSeconds(3f);

                    while (Nav.speed < 30)
                    {
                        Nav.speed += 0.1f;
                        yield return new WaitForSeconds(0.1f);
                    }

                    break;

                case 3:

                    Nav.speed = 40;

                    yield return new WaitForSeconds(3f);

                    Nav.speed = 30;

                    break;


            }
        }

    }
}
