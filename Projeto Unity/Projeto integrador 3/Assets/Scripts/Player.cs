using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;
    public static GameObject MinePlayer;

    public float Speed;
    public float PowerUpSpeed;
    public float SpeedLocal;
    public float SpeedRotation;
    public float SpeedFire;
    
    public GameObject FirePadrao;
    public GameObject GunL;
    public GameObject GunR;
    public GameObject PowerFire;
    public GameObject PrefabPlayerName;

    public Animator AnimatorBufDebuf;

    Rigidbody rb;
    Rigidbody FireRb;

    GameObject fireLocal;
    GameObject FireStandart;
    GameObject ObjectSpriteBufDebuf;

    CinemachineVirtualCamera Camera;

    Image ImageBufDebuf;

    Vector3 PosAnterior;
    Vector3 PosAnterior2;

    float hori;
    float vert;
    float Aceleração = 0;



    bool IsFiring;
    bool IsFiringAux;
    bool Slow = false;
    bool ControlInvert = false;
    bool auxVoltar = false;
    bool auxSpeed = false;

    int Sortear;
    int AuxAnim;
    int auxWhile = 0;

    List<Vector3> Posicoes = new List<Vector3>();

    Vector3 MousePosition;

    Ray ray;

    RaycastHit hit;

    ClassificacaoController classificacao;

    Text Classificação;

    private void Awake()
    {

        if (photonView.IsMine)
        {
            Camera = GameObject.FindGameObjectWithTag("cine").GetComponent<CinemachineVirtualCamera>();
            Camera.Follow = transform;
            Camera.LookAt = transform;

            rb = GetComponent<Rigidbody>();

            ObjectSpriteBufDebuf = GameObject.Find("ImgDufDebuf");

            ImageBufDebuf = ObjectSpriteBufDebuf.GetComponent<Image>();

            AnimatorBufDebuf = ObjectSpriteBufDebuf.GetComponent<Animator>();

            ObjectSpriteBufDebuf.SetActive(false);

            LocalPlayerInstance = this.gameObject;


            this.gameObject.name = PhotonNetwork.NickName.ToString();

        }

        this.gameObject.name = this.photonView.Owner.NickName;
    }

    void Start()
    {

        if (photonView.IsMine)
        {
            Camera = GameObject.FindGameObjectWithTag("cine").GetComponent<CinemachineVirtualCamera>();

            Camera.Follow = transform;
            Camera.LookAt = transform;



            rb = GetComponent<Rigidbody>();

            LocalPlayerInstance = this.gameObject;

            classificacao = GameObject.Find("AuxClas").GetComponent<ClassificacaoController>();
            Classificação = GameObject.Find("TextClass").GetComponent<Text>();

        }

        //GameObject playerUI = Instantiate(PrefabPlayerUI);
        //playerUI.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        GameObject playerName = Instantiate(PrefabPlayerName);
        playerName.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

        StartCoroutine(Voltar());
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

                MinePlayer = this.gameObject;

                if (!ControlInvert)
                    transform.Rotate(new Vector3(0, hori, 0) * SpeedRotation);
                else
                    transform.Rotate(new Vector3(0, -hori, 0) * SpeedRotation);


                rb.velocity = transform.forward * Aceleração * SpeedLocal;

                if (Input.GetKeyDown(KeyCode.Space) && !IsFiring)
                {
                    IsFiring = true;
                    StartCoroutine(Atirar());

                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    IsFiring = false;
                    StopCoroutine(Atirar());
                }

                for (int x = 0;x < classificacao.Jogadores.Count; x++){

                    if (classificacao.Jogadores[x] == gameObject)
                    {
                        // coloque aqui o text classificação;
                        Classificação.text = (1 + x).ToString() + "/" + classificacao.Jogadores.Count;
                    }

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

            if(Physics.Raycast(transform.position,transform.up * -1, out hit))
            {
                if (hit.collider == null)
                {
                    //transform.position = Posicoes[0];

                }
                if(!auxSpeed)
                    SpeedLocal = Speed;
            }
            else
            {
                SpeedLocal = 10;

                //transform.position = Posicoes[0];
                //Aceleração = 0;
            }

        }
    }

    private IEnumerator Voltar()
    {
        Posicoes.Add(transform.position);
        yield return new WaitForSeconds(0.1f);
        if(Posicoes.Count > 4)
        {
            Posicoes.Remove(Posicoes[0]);
        }
        StartCoroutine(Voltar());
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

        fireLocal = Instantiate(FireStandart, GunL.transform.position, transform.rotation);
        FireRb = fireLocal.GetComponent<Rigidbody>();
        FireRb.velocity = fireLocal.transform.forward * SpeedFire;
        fireLocal = Instantiate(FireStandart, GunR.transform.position, transform.rotation);
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
            auxSpeed = true;
            while(SpeedLocal > 5)
            {

                SpeedLocal--; 
            }
            Slow = false;
            while (SpeedLocal < 20)
            {
                yield return new WaitForSeconds(0.2f);
                SpeedLocal++;
            }
            auxSpeed = false;

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
            print("aqui");
            auxSpeed = true;
            SpeedLocal = PowerUpSpeed;
            while(SpeedLocal > Speed)
            {
                SpeedLocal -= 1;
                yield return new WaitForSeconds(0.01f);

                if (auxWhile > 100)
                {
                    break;
                }
                auxWhile++;
            }
            auxSpeed = false;

        } else if (other != null && other.CompareTag("PowerUp"))
        {
            if(photonView.IsMine)
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
                auxSpeed = true;
                SpeedLocal = 10;

                yield return new WaitForSeconds(3f);

                SpeedLocal = Speed;
                auxSpeed = false;

                yield return new WaitForSeconds(1f);

                ObjectSpriteBufDebuf.SetActive(false);

            }
            else if (AuxAnim == 1)
            {
                auxSpeed = true;
                SpeedLocal = 40;

                yield return new WaitForSeconds(3f);

                SpeedLocal = 20;
                auxSpeed = false;

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
        else if (other != null && other.CompareTag("chegada"))
        {
            


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
