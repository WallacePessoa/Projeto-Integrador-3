using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Laser : MonoBehaviourPun
{

    void Start()
    {

        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
