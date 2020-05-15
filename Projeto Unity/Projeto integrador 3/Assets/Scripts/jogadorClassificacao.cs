using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class jogadorClassificacao : MonoBehaviour
{

    public Text TextClassificação;

    NavMeshAgent navMesh;
    GameObject chegada;

    float Aux;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        chegada = GameObject.FindGameObjectWithTag("Chegada");
        StartCoroutine(classificar());

        navMesh.destination = chegada.transform.position;
        print(navMesh.destination);
        print(navMesh.remainingDistance);
        GameManager.Instace.Classifição.Add(navMesh.remainingDistance);
        print(navMesh.remainingDistance);
        Aux = navMesh.remainingDistance;

    }

    // Update is called once per frame
    void Update()
    {
        print(navMesh.destination);
        print(navMesh.remainingDistance);
    }


    public IEnumerator classificar()
    {


        Aux = navMesh.remainingDistance;
        for (int f = 0; f < GameManager.Instace.Classifição.Count; f++)
        {
            if (Aux == GameManager.Instace.Classifição[f])
            {
                GameManager.Instace.Classifição[f] = navMesh.remainingDistance;
                TextClassificação.text = GameManager.Instace.Classifição.Count.ToString() + "/" + f;                
            }
        }
        
        yield return new WaitForSeconds(1f);

        StartCoroutine(classificar());
    }
}
