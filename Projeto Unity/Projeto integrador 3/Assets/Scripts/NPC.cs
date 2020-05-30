using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class NPC : MonoBehaviourPun, IPunObservable
{

    public float Speed;
    public float SpeedLocal;
    public float SpeedFire;
    public float PowerUpSpeed;

    public Vector3 PosDestino;

    public GameObject[] QtdPositions;

    public GameObject FirePadrao;
    public GameObject PowerFire;
    public GameObject GunL;
    public GameObject GunR;
    public GameObject PontoEscolhido;

    GameObject fireLocal;
    GameObject FireStandart;
    GameObject Position;

    Rigidbody FireRb;


    float hori;
    float vert;
    public float Aceleração;
    public int Destino;
    int Sortear;

    bool IsFiring;

    bool IsFiringAux;

    bool AuxStart;


    NavMeshAgent Nav;

    Ray ray;

    RaycastHit hit;

    public List<GameObject> Pos = new List<GameObject>();

    public enum StateMachine
    {
        Correr,
        Curva,
        Atirar
    }

    public StateMachine Estado;
    private int auxWhile;
    public bool Slow = false;

    void Start()
    {

        QtdPositions = GameObject.FindGameObjectsWithTag("Nodes");
        int aux = 0;
        int aux2 = 0;
        while(QtdPositions.Length != Pos.Count)
        {
            Position = GameObject.Find("Node (" + aux + ")");
            Pos.Add(Position);
            aux++;

            if (aux2 > 100)
            {
                Debug.LogError("<Color=Red>excedeu</Color>");
                break;
            }
            else
                aux2++;

        }
       

        Nav = GetComponent<NavMeshAgent>();
        Estado = StateMachine.Correr;

        PosDestino = Vector3.zero;

        StartCoroutine(StartRun());

        SpeedLocal = Speed;
        Destino = 0;
        AuxStart = false;
        Nav.speed = 0;
        Aceleração = 0;
        PosDestino = new Vector3(Pos[Destino].transform.position.x + Random.Range(-10, 10), Pos[Destino].transform.position.y, Pos[Destino].transform.position.z + Random.Range(-10, 10));

    }

    // Update is called once per frame
    void  FixedUpdate()
    {
        if (GameManager.Instace.StartGame == true && AuxStart)
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

                //Nav.speed = SpeedLocal;

                switch (Estado)
                {
                    case StateMachine.Correr:



                        Nav.destination = PosDestino;

                        Nav.speed = SpeedLocal;

                        if (Vector3.Distance(transform.position, PosDestino) < 5)
                        {
                            Estado = StateMachine.Curva;
                        }
                        break;

                    case StateMachine.Curva:

                        if (Destino + 1 < Pos.Count)
                        {
                            Destino++;
                        }


                        PosDestino = new Vector3(Pos[Destino].transform.position.x + Random.Range(-10,10), Pos[Destino].transform.position.y, Pos[Destino].transform.position.z + Random.Range(-10,10));
                        Estado = StateMachine.Correr;

                        break;

                    case StateMachine.Atirar:

                        //Nav.destination = PosDestino;



                        if (Vector3.Distance(transform.position, PosDestino) < 5)
                        {
                            Estado = StateMachine.Curva;
                        }

                        break;
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

           


        }




    }

    private IEnumerator StartRun()
    {
        if (GameManager.Instace.StartGame == true)
        {
            //PontoEscolhido = Positions[Destino];

            FireStandart = FirePadrao;

            
            AuxStart = true;
        }
        else
        {
            yield return null;
            StartCoroutine(StartRun());
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
        if (Aceleração < SpeedLocal)
        {
            Aceleração += 0.1f;

            yield return new WaitForSeconds(0.05f);

        }
        else
        {
            //Nav.speed = SpeedLocal * Time.deltaTime;
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
            if (SpeedLocal > 5)
            {
                SpeedLocal -= 0.1f * Time.deltaTime;
                yield return new WaitForSeconds(2f);
                SpeedLocal += 0.1f * Time.deltaTime;
            }
            if (other != null)
                Destroy(other.gameObject);


        }
        else if (other != null && other.CompareTag("PowerLaser"))
        {

            if (SpeedLocal > 9)
            {
                SpeedLocal -= 0.1f * Time.deltaTime;
                yield return new WaitForSeconds(2f);
                SpeedLocal += 0.1f * Time.deltaTime;
            }



        }
        else if (other != null && other.CompareTag("PowerUp"))
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

                    SpeedLocal = 10;

                    yield return new WaitForSeconds(3f);

                    SpeedLocal = Speed;

                    break;

                case 3:

                    Nav.speed = 40 * Time.deltaTime;

                    yield return new WaitForSeconds(3f);

                    SpeedLocal = Speed * Time.deltaTime;

                    break;


            }
        }
        else if (other != null && other.CompareTag("PowerUpSpeed"))
        {

            SpeedLocal = PowerUpSpeed;
            while (SpeedLocal > Speed)
            {
                //rb.velocity = new Vector3(-hori, 0, -vert) * SpeedLocal;
                Nav.speed = SpeedLocal * Time.deltaTime;
                SpeedLocal -= 1;
                yield return new WaitForSeconds(0.01f);

                if (auxWhile > 100)
                {
                    break;
                }
                auxWhile++;
            }
        }
        else if (other != null && other.CompareTag("Obstaculo"))
        {
            Slow = true;
            //print("entrou aui");
            SpeedLocal = 5;
            while (Slow == true)
            {
                yield return new WaitForSeconds(0.5f);
            }

            SpeedLocal = Speed;
        }



    }

    private IEnumerator OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstaculo"))
        {
            Slow = false;
            yield return null;
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
