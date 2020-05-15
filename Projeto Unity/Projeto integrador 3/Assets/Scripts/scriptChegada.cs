﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class scriptChegada : MonoBehaviourPun
{
    public Image Painel;
    public Text ClassificaçãoJogadores;
    public Button Back;

    private List<string> TextJogadoresClassificados = new List<string>();

    bool AuxClassificacao;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Painel.gameObject.activeSelf && AuxClassificacao)
        {
            AuxClassificacao = false;

            int contador = 1;
            foreach (string name in TextJogadoresClassificados)
            {
                ClassificaçãoJogadores.text = ClassificaçãoJogadores.text + "\n" + contador.ToString() + " " + name;
                contador++;
            }
                        
            Back.gameObject.SetActive(true);
        }
    }

    public void back()
    {
        PhotonNetwork.LoadLevel("Menu");
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
                Painel.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(1f);

            other.gameObject.SetActive(false);

        }
    }

}
