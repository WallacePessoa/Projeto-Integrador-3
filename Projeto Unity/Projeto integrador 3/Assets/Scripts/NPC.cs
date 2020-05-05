using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class NPC : MonoBehaviourPun, IPunObservable
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

    bool IsFiring;

    bool IsFiringAux;


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
        // StartCoroutine(Acelerar());
        Nav = GetComponent<NavMeshAgent>();
        Estado = StateMachine.Correr;
       
        Positions = GameObject.FindGameObjectsWithTag("Nodes");


        PosDestino = new Vector3(Positions[Destino].transform.position.x + Random.Range(-10, 10), Positions[Destino].transform.position.y, Positions[Destino].transform.position.z + Random.Range(-10, 10));

        //for (int x = 0; x< Positions.Length;x++)
        //    Posiçoes[x] = Positions[x].transform.position;

        FireStandart = FirePadrao;

        Nav.speed = Speed;

    }

    // Update is called once per frame
    void Update()
    {

        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.DrawRay(transform.position, hit.transform.forward * 20, Color.red);

                if (hit.collider.CompareTag("Jogador") && hit.collider.gameObject != gameObject && !IsFiring)
                {
                    IsFiring = true;
                    StartCoroutine(atirar());
                }
                else if (!IsFiring)
                {
                    IsFiring = false;
                    StopCoroutine(atirar());
                }

            }
            else
            {
                IsFiring = false;
                StopCoroutine(atirar());
            }

        }

        if (IsFiring && !IsFiringAux)
        {
            IsFiringAux = true;
            StartCoroutine(atirar());
        }

        else if (!IsFiring)
        {
            IsFiringAux = false;
            StopCoroutine(atirar());
        }



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



                if (Vector3.Distance(transform.position, PosDestino) < 5)
                {
                    Estado = StateMachine.Curva;
                }

                break;
        }   

    }

    private IEnumerator atirar()
    {
        fireLocal = Instantiate(FireStandart, GunL.transform.position, transform.rotation);

        FireRb = fireLocal.GetComponent<Rigidbody>();

        FireRb.velocity = transform.forward * SpeedFire;

        fireLocal = Instantiate(FireStandart, GunR.transform.position, transform.rotation);

        FireRb = fireLocal.GetComponent<Rigidbody>();

        FireRb.velocity = transform.forward * SpeedFire;

        yield return new WaitForSeconds(1f);

        if (IsFiring)
            StartCoroutine(atirar());
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
            StopCoroutine(Acelerar());
        }



        yield return null;
        StartCoroutine(Acelerar());
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Laser"))
        {
            //print(other.gameObject != null);
            //print("A AI " + gameObject.name + " está lenta");
            if (Nav.speed > 5)
            {
                Nav.speed -= 0.1f;
                yield return new WaitForSeconds(2f);
                Nav.speed += 0.1f;
            }
            if(other != null)
                Destroy(other.gameObject);


        }else if (other != null && other.CompareTag("PowerLaser"))
        {

            if (Nav.speed > 9)
            {
                Nav.speed -= 0.1f;
                yield return new WaitForSeconds(2f);
                Nav.speed += 0.1f;
            }

            

        }else if (other != null && other.CompareTag("PowerUp"))
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
        else if (other != null && other.CompareTag("Chegada"))
        {
            GameManager.Instace.hudWim(gameObject.name);
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsFiring);
        }
        else
        {
            this.IsFiring = (bool)stream.ReceiveNext();
        }
    }
}
