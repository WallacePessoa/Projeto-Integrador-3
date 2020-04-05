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
    private float IntTime;
    [SerializeField]
    private Image LoadPanel;


    #endregion
    #region Private Fields
    string gameVersion = "1";
    bool isConnect;
    string Fase;
    bool aux = false;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        LoadPanel.gameObject.SetActive(false);
    }
    void Start()
    {

        IntTime = 10;
        TextTime.text = IntTime.ToString();

 


    }

    void FixedUpdate()
    {


        if (PhotonNetwork.IsConnected)
        {//Está conectado... entra em uma sala aleatoriamente
            IntTime -= Time.deltaTime;
            TextTime.text = IntTime.ToString();

        }
        if (IntTime <= 0 && !aux)
        {
            aux = !aux;
            OnJoinedRoom();
        }

        PlayerCont.text = PhotonNetwork.CountOfPlayersOnMaster.ToString();
    }
    #endregion

    #region Public Methods
    public void Connect(string fase)
    {
        Fase = fase;
        LoadPanel.gameObject.SetActive(true);



        if (PhotonNetwork.IsConnected)
        {//Está conectado... entra em uma sala aleatoriamente
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinRoom(fase);

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
            //PhotonNetwork.JoinRandomRoom();
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
    }

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    Debug.Log("Falhou ao se conectar com uma sala... Criando nova sala");
    //    PhotonNetwork.CreateRoom(Fase, new RoomOptions { MaxPlayers = maxPlayers});
    //}

    public override void OnJoinedRoom()
    {
        if(IntTime <= 0)
        {
            Debug.Log("Conectado na sala: " + PhotonNetwork.CurrentRoom);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                PhotonNetwork.LoadLevel(Fase);
        }


        

    }
    #endregion
}
