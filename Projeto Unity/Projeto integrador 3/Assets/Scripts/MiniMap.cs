using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class MiniMap : MonoBehaviour
{
    public GameObject cam;
    public GameObject miniCam;
    public GameObject player;
    public int test = 1;

    Vector3 NewPosition;
    Vector3 vector;

    void Start()
    {
        player = GameObject.Find(PhotonNetwork.NickName);

        NewPosition.y = 100;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        vector = miniCam.transform.position - player.transform.position;
        NewPosition.x = cam.transform.position.x;
        NewPosition.z = cam.transform.position.z;
        //transform.rotation = cam.transform.rotation;
        transform.position = vector / test;
    }
}
