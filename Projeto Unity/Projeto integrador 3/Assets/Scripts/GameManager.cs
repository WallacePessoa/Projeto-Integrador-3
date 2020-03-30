using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameManager Instace;

    public Transform[] Posiçõeslargada;

    public List<GameObject> Jogadores = new List<GameObject>();

    public GameObject Player;
    public GameObject Chegada;

    public GameObject IA;

    public Transform[] Classifição;

    float Distancia1;
    float Distancia2;

    NavMesh nav;


    int PosPlayer;

    private void Awake()
    {
        if(Instace == null)
        {
            Instace = this;
        }

        PosPlayer = Random.Range(0, Posiçõeslargada.Length);

        Player.transform.position = Posiçõeslargada[PosPlayer].position;

        foreach(Transform pos in Posiçõeslargada)
        {
            if (pos.position != Player.transform.position)
            {
                Jogadores.Add(Instantiate(IA, pos.position, transform.rotation));
            }
            else
                Jogadores.Add(Player);
        }

        for (int x = 0; x < Classifição.Length; x++)
        {
            Classifição[x] = Jogadores[x].transform;
        }

    }

    private void FixedUpdate()
    {

        foreach (Transform clas in Classifição)
        {
            Distancia1 = Vector3.Distance(clas.position, Chegada.transform.position);


        }

        for(int y =0; y < Classifição.Length; y++)
        {
            Distancia1 = Vector3.Distance(Classifição[y].position, Chegada.transform.position);

            for (int x = 0; x < Classifição.Length; x++)
            {
                Distancia2 = Vector3.Distance(Classifição[x].position, Chegada.transform.position);

                if (Distancia1 < Distancia2)
                {
                    Transform aux = Classifição[y];
                    Classifição[y] = Classifição[x];
                    Classifição[x] = aux;
                }
            }

        }

    }


  

}
