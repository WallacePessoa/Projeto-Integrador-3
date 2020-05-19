using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class scriptChegada : MonoBehaviourPun
{
    public GameObject GamePainel;
    public Text ClassificaçãoJogadores;

    private List<string> TextJogadoresClassificados = new List<string>();

    bool AuxClassificacao;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GamePainel.activeSelf && AuxClassificacao)
        {
            AuxClassificacao = false;

            int contador = 1;
            foreach (string name in TextJogadoresClassificados)
            {
                ClassificaçãoJogadores.text = ClassificaçãoJogadores.text + "\n\n" + contador.ToString() + " " + name;
                contador++;
            }
                        
        }
    }

    public void back()
    {
        PhotonNetwork.LoadLevel("Menu");
        PhotonNetwork.LeaveRoom();
        print("saiu da sala");
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jogador"))
        {
            AuxClassificacao = true;
            ClassificaçãoJogadores.text = "";
            TextJogadoresClassificados.Add(other.gameObject.name);
            
            if (other.gameObject == Player.MinePlayer)
            {
                GamePainel.SetActive(true);
            }
            yield return new WaitForSeconds(1f);

            other.gameObject.SetActive(false);

        }
    }

}
