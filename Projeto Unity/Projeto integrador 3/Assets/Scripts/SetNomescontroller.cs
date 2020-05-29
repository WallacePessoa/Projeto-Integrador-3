using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SetNomescontroller : MonoBehaviourPun
{

    public Text TextNomes;


    [PunRPC]
    void NomesMenssager(string Nomes)
    {

        TextNomes.text = TextNomes.text + "\n" + Nomes;
        
        
    }

    public void SendNomesMenssager(string Nomes)
    {
        photonView.RPC("NomesMenssager", RpcTarget.All, Nomes);
    }




}
