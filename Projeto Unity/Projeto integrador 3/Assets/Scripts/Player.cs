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


    public static GameObject LocalPlayerInstance;

    Rigidbody rb;
    Rigidbody FireRb;

    GameObject fireLocal;
    GameObject FireStandart;
    GameObject ObjectSpriteBufDebuf;

    private static GameManager gameManager;


    CinemachineVirtualCamera Camera;

    Image ImageBufDebuf;
    public Animator AnimatorBufDebuf;

    float hori;
    public float vert;

    public float Aceleração = 0;

    bool IsFiring;

    bool IsFiringAux;

    int Sortear;

    bool Slow = false;

    bool ControlInvert = false;

    Vector3 MousePosition;

    Ray ray;

    RaycastHit hit;

    int AuxAnim;
    int auxWhile = 0;

    private void Awake()
    {

        if (photonView.IsMine)
        {
            Camera = GameObject.FindGameObjectWithTag("cine").GetComponent<CinemachineVirtualCamera>();
            Camera.Follow = transform;
            Camera.LookAt = transform;

            CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();

            rb = GetComponent<Rigidbody>();

            ObjectSpriteBufDebuf = GameObject.Find("ImgDufDebuf");

            ImageBufDebuf = ObjectSpriteBufDebuf.GetComponent<Image>();

            AnimatorBufDebuf = ObjectSpriteBufDebuf.GetComponent<Animator>();

            ObjectSpriteBufDebuf.SetActive(false);

            LocalPlayerInstance = this.gameObject;

        }

    }

    void Start()
    {
        StartCoroutine(StartRun());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.Instace.StartGame == true)
        {
            if (photonView.IsMine)
            {

                hori = Input.GetAxis("Horizontal");
                vert = Input.GetAxis("Vertical");



                if (!ControlInvert)
                    transform.Rotate(new Vector3(0, hori, 0) * SpeedRotation);
                else
                    transform.Rotate(new Vector3(0, -hori, 0) * SpeedRotation);


                rb.velocity = transform.forward * Aceleração * SpeedLocal;

                if (Input.GetKeyDown(KeyCode.Mouse0) && !IsFiring)
                {
                    IsFiring = true;
                    StartCoroutine(Atirar());

                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
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
                else if (!IsFiring)
                {
                    IsFiringAux = false;
                }
            }
        }
    }

    private IEnumerator StartRun()
    {
        if (GameManager.Instace.StartGame == true)
        {
            if (photonView.IsMine)
            {

                SpeedLocal = Speed;

                FireStandart = FirePadrao;
                StartCoroutine(Acelerar());

                if (gameManager == null)
                {
                    gameManager = FindObjectOfType<GameManager>();
                }
            }

        }
        else
        {
            yield return null;
            StartCoroutine(StartRun());
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
        if (IsFiring)
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
            ObjectSpriteBufDebuf.SetActive(true);

            

            yield return new WaitForSeconds(Random.Range(1f, 3f));
            AuxAnim = Random.Range(1, 5);
            AnimatorBufDebuf.SetBool(AuxAnim.ToString(), true);


            
            if(AuxAnim == 3)
            {
                FireStandart = PowerFire;

                yield return new WaitForSeconds(3f);

                FireStandart = FirePadrao;

                yield return new WaitForSeconds(1f);

                ObjectSpriteBufDebuf.SetActive(false);
            }
            else if (AuxAnim == 2)
            {
                SpeedLocal = 10;

                yield return new WaitForSeconds(3f);

                SpeedLocal = Speed;

                yield return new WaitForSeconds(1f);

                ObjectSpriteBufDebuf.SetActive(false);
            }
            else if (AuxAnim == 1)
            {
                SpeedLocal = 40;

                yield return new WaitForSeconds(3f);

                SpeedLocal = 20;

                yield return new WaitForSeconds(1f);

                ObjectSpriteBufDebuf.SetActive(false);
            }
            else if (AuxAnim == 4)
            {
                ControlInvert = true;

                yield return new WaitForSeconds(3f);

                ControlInvert = false;

                yield return new WaitForSeconds(1f);

                ObjectSpriteBufDebuf.SetActive(false);
            }
            else
            {
                print("BufDebuf não encontrado");
                print(ImageBufDebuf.name);
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
