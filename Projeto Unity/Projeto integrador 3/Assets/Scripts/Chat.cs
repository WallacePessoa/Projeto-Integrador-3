using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Chat : MonoBehaviourPun
{
    public Text TextChat;

    [PunRPC]
    void ChatMenssager(string Usuario, string mensagem)
    {
        TextChat.text = Usuario + ": " + mensagem + "\n" + "\n" + TextChat.text;
    }

    public void SendChatMenssager(string usuario, string mensagem)
    {
        photonView.RPC("ChatMenssager", RpcTarget.All, usuario, mensagem);
    }
}
