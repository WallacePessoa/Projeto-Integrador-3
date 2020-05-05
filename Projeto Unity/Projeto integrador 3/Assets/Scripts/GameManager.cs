using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager Instace;

    public GameObject[] Posiçõeslargada;

    public List<GameObject> Jogadores = new List<GameObject>();

    public GameObject PlayerPrefab;
    public GameObject Chegada;

    public int PlayerID;

    public GameObject IA;

    public List<float> Classifição = new List<float>();

    public GameObject[] JogadoresOnline;



    GameObject MeuPlayer;

    [SerializeField]
    private Text Ganhador;
    [SerializeField]
    private Image Panel;
    [SerializeField]
    private Button Back;
    [SerializeField]
    private Text StartRum;

    float Distancia1;
    float Distancia2;

    int AuxAlterarPosição;
    int AuxJogador;

    int PosPlayer;

    private void Awake()
    {
        if(Instace == null)
        {
            Instace = this;
        }

        int aux = PlayerIndex.instancePlayerIndex.playerId - 1;

        Debug.Log(aux);

        if (Player.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(PlayerPrefab.name, Posiçõeslargada[aux].transform.position, Posiçõeslargada[aux].transform.rotation);
            Debug.Log("Player intanciado em " + Application.loadedLevelName);
            
        }

        int x = PhotonNetwork.CurrentRoom.PlayerCount;
        print(x);

        for (int y = x; y < Posiçõeslargada.Length; y++)
        {
            PhotonNetwork.Instantiate(IA.name, Posiçõeslargada[y].transform.position, Posiçõeslargada[y].transform.rotation);
        }


    }

    private void FixedUpdate()
    {

         
    }


    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //executa quando outro jogador entra na sala.
        Debug.Log("O player " + newPlayer.NickName + " entrou na sala");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Você é o cliente mestre");
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //executa quando um jogador que não é o meu sai da sala;
        Debug.Log("O player " + otherPlayer.NickName + " saiu da sala");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Você é o cliente mestre!!!");

            //LoadArena();

        }
    }

    #endregion


    #region Public Methods
    public void hudWim(string Name)
    {
        Time.timeScale = 0;
        Ganhador.text = "O jogador " + Name + " venceu";

        Panel.gameObject.SetActive(true);
        Back.gameObject.SetActive(true);
        Ganhador.gameObject.SetActive(true);
    }

    public void BackRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Private Mathods
    void LoadArena()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Você não é o cliente Mestre");
        }
        PhotonNetwork.LoadLevel("Fase_" + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
    #endregion

}
