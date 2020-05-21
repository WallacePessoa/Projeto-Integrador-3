using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class MiniMap : MonoBehaviour
{
    public GameObject cam;

    Vector3 NewPosition;

    void Start()
    {


        NewPosition.y = 120;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        NewPosition.x = cam.transform.position.x;
        NewPosition.z = cam.transform.position.z;
        transform.rotation = cam.transform.rotation;
        transform.position = NewPosition;
    }
}
