using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassificacaoController : MonoBehaviour
{
    public GameObject PontoFixo;
    public GameObject ObjectAux;

    public List<GameObject> Jogadores = new List<GameObject>();
    List<GameObject> Pais = new List<GameObject>();

    bool PrimeiroJogador = false;
    bool PrimeiroJogadorLista = true;

    // Start is called before the first frame update
    void Start()
    {
        Jogadores.AddRange(GameObject.FindGameObjectsWithTag("Jogador"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (PrimeiroJogador)
        {
            foreach (GameObject game in Pais)
            {
                if (game == Pais[0] && PrimeiroJogadorLista)
                {
                    PrimeiroJogadorLista = false;
                    ObjectAux = game;
                }
                else
                {
                    if (Vector3.Distance(ObjectAux.transform.position, PontoFixo.transform.position) > Vector3.Distance(game.transform.position, PontoFixo.transform.position))
                    {
                        ObjectAux = game;
                        //transform.SetParent(ObjectAux.gameObject.transform, true);
                    }
                }


            }
            transform.position = ObjectAux.transform.position;
            transform.rotation = ObjectAux.transform.rotation;


            Jogadores.Sort((a, b) => (Vector3.Distance(a.transform.position, PontoFixo.transform.position).CompareTo(Vector3.Distance( b.transform.position, PontoFixo.transform.position))));

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jogador"))
        {
            Pais.Add(other.gameObject);
            PrimeiroJogador = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jogador"))
        {
            Pais.Remove(other.gameObject);

        }
    }
}
