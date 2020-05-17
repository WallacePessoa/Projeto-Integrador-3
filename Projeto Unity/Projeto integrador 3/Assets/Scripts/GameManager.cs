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

    public bool StartGame = false;



    GameObject MeuPlayer;

    [SerializeField]
    private Text Ganhador;
    [SerializeField]
    private Image Panel;
    [SerializeField]
    private Button Back;
    [SerializeField]
    private Text TextStartTime;

    float Distancia1;
    float Distancia2;

    int AuxAlterarPosição;
    int AuxJogador;
    int IntStartTime = 5;
    int PosPlayer;

    private void Awake()
    {
        StartGame = false;
        if(Instace == null)
        {
            Instace = this;
        }

        int aux = PlayerIndex.instancePlayerIndex.playerId - 1;

        Debug.Log(aux);

        if (Player.LocalPlayerInstance == null)
        {
            GameObject game = PhotonNetwork.Instantiate(PlayerPrefab.name, Posiçõeslargada[aux].transform.position, Posiçõeslargada[aux].transform.rotation);
            game.name = PhotonNetwork.NickName;
            
        }

        int x = PhotonNetwork.CurrentRoom.PlayerCount;
        print(x);
        if (PhotonNetwork.IsMasterClient)
        {
            for (int y = x; y < Posiçõeslargada.Length; y++)
            {
                PhotonNetwork.Instantiate(IA.name, Posiçõeslargada[y].transform.position, Posiçõeslargada[y].transform.rotation);
            }
        }

        StartCoroutine(StarTime());
    }

    private void FixedUpdate()
    {

        int aux2 = 10;
        aux2 = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["start"].ToString());
        print(aux2);
        print("Numero de jogadores" + PhotonNetwork.CurrentRoom.PlayerCount);
        print(PhotonNetwork.CurrentRoom.CustomProperties["start"].ToString());

        if(aux2 == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            TextStartTime.gameObject.SetActive(false);
            StartGame = true;
        }

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

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //executa quando um jogador que não é o meu sai da sala;
        Debug.Log("O player " + otherPlayer.NickName + " saiu da sala");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Você é o cliente mestre!!!");

        }
    }

    #endregion


    #region Public Methods

    public IEnumerator StarTime()
    {
        TextStartTime.text = IntStartTime.ToString();



        yield return new WaitForSeconds(1f);
        IntStartTime--;

        if(IntStartTime !=0)
        {


            StartCoroutine(StarTime());
        }
        else
        {
            TextStartTime.text = IntStartTime.ToString();

            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

            int aux = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["start"].ToString());
            aux++;

            hash.Add("start", aux);

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

    }
    public void hudWim(string Name)
    {
        
        Ganhador.text = "O jogador " + Name + " venceu";

        Panel.gameObject.SetActive(true);
        Back.gameObject.SetActive(true);
        Ganhador.gameObject.SetActive(true);
    }

    public void BackRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Menu");
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
    #endregion

    #region Private Mathods


    #endregion

}
