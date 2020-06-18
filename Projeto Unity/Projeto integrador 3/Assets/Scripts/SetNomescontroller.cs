using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SetNomescontroller : MonoBehaviourPun
{

    public Text TextNomes;

    public List<string> Nome = new List<string>();

    [PunRPC]
    void NomesMenssager(string Nomes)
    {

        TextNomes.text = TextNomes.text + "\n" + "\n" + Nomes;
        
        
    }

    public void SendNomesMenssager(string Nomes)
    {
        TextNomes.text = "players: ";
        foreach (string i in Nome)
        {
            photonView.RPC("NomesMenssager", RpcTarget.All, i);
            print("aa");
        }

    }




}
