using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Launcher : MonoBehaviourPunCallbacks
{
    #region Serialize Fields
    [SerializeField]
    private byte maxPlayers = 4;
    [SerializeField]
    private Text TextTime;
    [SerializeField]
    private Text PlayerCont;
    [SerializeField]
    private float FloatTime = 0;
    [SerializeField]
    private Image LoadPanel;
    [SerializeField]
    private Button BtnJoinScene;
    [SerializeField]
    private Text TextJogadores;
    [SerializeField]
    private Text NomeJogadores;
    [SerializeField]
    private InputField InputFieldMensagem;
    [SerializeField]
    private GameObject ObjectChat;
    [SerializeField]
    private GameObject ChatPanel;
    [SerializeField]
    private GameObject ObjectNome;


    #endregion

    #region Private Fields

    int IntTime = 0;

    string gameVersion = "1";
    bool isConnect;
    string Fase;
    bool EntrarNaSala = false;
    bool AuxInput = true;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        LoadPanel.gameObject.SetActive(false);
    }
    void Start()
    {


        TextTime.text = IntTime.ToString();
        StartCoroutine(contar());


    }

    public IEnumerator contar()
    {
        if (EntrarNaSala)
        {
            if (FloatTime > IntTime)
            {
                IntTime++;
            }
            FloatTime += 1f;
            TextTime.text = "Tempo de espera: " + IntTime.ToString();

            PlayerCont.text = "Jogadores online: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(contar());
    }

    void FixedUpdate()
    {

        if (PhotonNetwork.IsConnected)
        {
            //Está conectado... entra em uma sala


            if (PhotonNetwork.IsMasterClient)
            {
                BtnJoinScene.gameObject.SetActive(true);
            }
            else
                BtnJoinScene.gameObject.SetActive(false);


            Debug.Log("Conectado na sala: " + PhotonNetwork.CurrentRoom);

            if (Input.GetKeyDown(KeyCode.Return) && AuxInput)
            {
                AuxInput = false;
                StartCoroutine(Inputs());
            }

            //NomeJogadores.text = "";

            //foreach (string i in PlayerIndex.instancePlayerIndex.nomes)
            //{
            //    NomeJogadores.text = NomeJogadores.text + "\n" + i;
            //}



        }
        if (PhotonNetwork.CountOfPlayersOnMaster == 8)
        {
            OnJoinedRoom();
        }
    }
    #endregion

    #region Public Methods

    public IEnumerator Inputs()
    {
        Chat chat = ObjectChat.GetComponent<Chat>();
        chat.SendChatMenssager(PhotonNetwork.LocalPlayer.NickName, InputFieldMensagem.text);
        InputFieldMensagem.text = "";
        yield return new WaitForSeconds(0.5f);
        AuxInput = true;
    }

    public void JoinScene()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("start", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        PhotonNetwork.LoadLevel(Fase);
    }

    public void Connect(string fase)
    {
        Fase = fase;
        LoadPanel.gameObject.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            //Está conectado... entra em uma sala
            PhotonNetwork.JoinRoom(Fase);
        }
        else
        {
            //não está conectado... cria a conexão com o Photon Server
            isConnect = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }


    }
    #endregion

    #region MonoBehaviourPunCallbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado no servidor Photon");
        if (isConnect)
        {
            PhotonNetwork.JoinRoom(Fase);
            isConnect = false;
        }

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Desconectado. Causa: " + cause);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Falhou ao se conectar com uma sala... Criando nova sala");
        PhotonNetwork.CreateRoom(Fase, new RoomOptions { MaxPlayers = maxPlayers });
        PlayerIndex.instancePlayerIndex.nomes.Add(PhotonNetwork.NickName);
        SetNomescontroller setNomescontroller = ObjectNome.GetComponent<SetNomescontroller>();
        setNomescontroller.SendNomesMenssager(PhotonNetwork.NickName);
        setNomescontroller.Nome.Add(PhotonNetwork.NickName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Conectado na sala: " + PhotonNetwork.CurrentRoom);

        PlayerIndex.instancePlayerIndex.playerId = PhotonNetwork.CurrentRoom.PlayerCount;
        ChatPanel.SetActive(true);

        EntrarNaSala = true;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerIndex.instancePlayerIndex.nomes.Add(newPlayer.NickName);
        SetNomescontroller setNomescontroller = ObjectNome.GetComponent<SetNomescontroller>();
        setNomescontroller.Nome.Add(newPlayer.NickName);
        setNomescontroller.SendNomesMenssager(newPlayer.NickName);
    }
    #endregion
}
