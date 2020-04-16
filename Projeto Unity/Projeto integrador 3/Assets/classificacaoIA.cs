using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class classificacaoIA : MonoBehaviour
{

    NavMeshAgent navMesh;
    GameObject chegada;

    float Aux;

    void Start()
    {
        
        navMesh = GetComponent<NavMeshAgent>();
        chegada = GameObject.FindGameObjectWithTag("Chegada");


        navMesh.destination = chegada.transform.position;
        GameManager.Instace.Classifição.Add(navMesh.remainingDistance);
        Aux = navMesh.remainingDistance;
        StartCoroutine(classificar());
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        
    }


    public IEnumerator classificar()
    {
        Aux = navMesh.remainingDistance;
        for(int f = 0;f< GameManager.Instace.Classifição.Count; f++)
        {
            if (Aux == GameManager.Instace.Classifição[f])
            {
                GameManager.Instace.Classifição[f] = navMesh.remainingDistance;
            }

        }


 

        yield return new WaitForSeconds(1f);

        StartCoroutine(classificar());
    }
}
