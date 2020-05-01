using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public float Speed;
    public float PowerUpSpeed;
    public float SpeedLocal;
    public float SpeedRotation;
    public float SpeedFire;

    public Text Classificação;

    public GameObject FirePadrao;
    public GameObject GunL;
    public GameObject GunR;
    public GameObject PowerFire;



    Rigidbody rb;
    Rigidbody FireRb;

    GameObject fireLocal;
    GameObject FireStandart;

    private static GameManager gameManager;
    public static GameObject LocalPlayerInstance;

    float hori;
    public float vert;

    public float Aceleração = 0;

    int Sortear;

    bool Slow = false;

    bool ControlInvert = false;

    bool IsFiring;

    bool IsFiringAux;

    Vector3 MousePosition;

    Ray ray;

    RaycastHit hit;

    public CinemachineVirtualCamera camera;


    int auxWhile = 0;

    private void Awake()
    {

        if (photonView.IsMine)
        {
            camera = GameObject.FindGameObjectWithTag("cine").GetComponent<CinemachineVirtualCamera>();
            camera.Follow = transform;
            camera.LookAt = transform;

            LocalPlayerInstance = this.gameObject;

            CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.Log("Player está sem o script camerawork", this);
            }
        }
    }

    void Start()
    {

        if (photonView.IsMine)
        {

            CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();
            rb = GetComponent<Rigidbody>();

            SpeedLocal = Speed;

            FireStandart = FirePadrao;

            StartCoroutine(Acelerar());

            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }

            if (cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.Log("Player está sem o script camerawork", this);
            }
        }

      
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            hori = Input.GetAxis("Horizontal");
            vert = Input.GetAxis("Vertical");

            transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);

            if (!ControlInvert)
                transform.Rotate(new Vector3(0, hori, 0) * SpeedRotation);
            else
                transform.Rotate(new Vector3(0, -hori, 0) * SpeedRotation);


            rb.velocity = transform.forward * Aceleração * SpeedLocal;


            if (Input.GetKeyDown(KeyCode.Mouse0) && !IsFiring)
            {
                IsFiring = true;
                StartCoroutine(Atirar());

            }else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                IsFiring = false;
                StopCoroutine(Atirar());
            }
        }
        else
        {

            if (IsFiring && !IsFiringAux)
            {
                IsFiringAux = true;
                StartCoroutine("Atirar");
            }
            else if(!IsFiring)
            {
                IsFiringAux = false; 
            }
        }


    }

    public IEnumerator Atirar()
    {

        fireLocal = Instantiate(FirePadrao, GunL.transform.position, transform.rotation);

        FireRb = fireLocal.GetComponent<Rigidbody>();

        FireRb.velocity = fireLocal.transform.forward * SpeedFire;

        fireLocal = Instantiate(FirePadrao, GunR.transform.position, transform.rotation);

        FireRb = fireLocal.GetComponent<Rigidbody>();

        FireRb.velocity = fireLocal.transform.forward * SpeedFire;

        yield return new WaitForSeconds(0.3f);
        if(IsFiring)
            StartCoroutine(Atirar());

    }

    private IEnumerator Acelerar()
    {


        if (Aceleração < vert && vert != 0)
        {
            Aceleração += 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
        else if (Aceleração > vert)
        {

            Aceleração -= 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
        else if (vert == 0)
            Aceleração = 0;
        yield return null;
        StartCoroutine(Acelerar());
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Laser"))
        {
            //print("Estou lento (player)");

            if(SpeedLocal > 19)
            {
                SpeedLocal -= 1;
                yield return new WaitForSeconds(3f);

                SpeedLocal += 1;
            }
            if(other!= null)
            {
                Destroy(other.gameObject);
            }


        }else if (other != null && other.CompareTag("Obstaculo"))
        {
            Slow = true;
            while(SpeedLocal > 5)
            {

                SpeedLocal--; 
            }
//            yield return new WaitForSeconds(0.5f);
            Slow = false;
            while (SpeedLocal < 20)
            {
                yield return new WaitForSeconds(0.2f);
                SpeedLocal++;
            }

        } else if (other != null && other.CompareTag("PowerLaser"))
        {

            if (SpeedLocal > 9)
            {
                SpeedLocal -= 1;
                yield return new WaitForSeconds(3f);

                SpeedLocal += 1;
            }

            

        }else if (other != null && other.CompareTag("PowerUpSpeed"))
        {

            SpeedLocal = PowerUpSpeed;
            while(SpeedLocal > Speed)
            {
                //rb.velocity = new Vector3(-hori, 0, -vert) * SpeedLocal;
                rb.velocity = transform.forward * vert * SpeedLocal;
                SpeedLocal -= 1;
                yield return new WaitForSeconds(0.01f);

                if (auxWhile > 100)
                {
                    break;
                }
                auxWhile++;
            }

        } else if (other != null && other.CompareTag("PowerUp"))
        {
            Sortear = Random.Range(1, 5);
            print(Sortear);

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

                    SpeedLocal = 40;

                    yield return new WaitForSeconds(3f);

                    SpeedLocal = 20;

                    break;

                case 4:


                    ControlInvert = true;

                    yield return new WaitForSeconds(3f);

                    ControlInvert = false;
                    break;



            }


        }
        else if (other != null && other.CompareTag("Chegada"))
        {
            GameManager.Instace.hudWim(gameObject.name);
        }


    }

    private IEnumerator OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstaculo"))
        {

            if (other.CompareTag("Obstaculo"))
            {
                if(Slow == false)
                {
                    while (SpeedLocal < 20)
                    {
                        yield return new WaitForSeconds(0.2f);
                        SpeedLocal++;
                    }
                }
                
            }
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
