using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookArmNPC : MonoBehaviour
{
    public NPC NPC;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jogador"))
        {
            NPC.Estado = NPC.StateMachine.Atirar;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jogador"))
        {
            NPC.Estado = NPC.StateMachine.Correr;
        }
    }
}
