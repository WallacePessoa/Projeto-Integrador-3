using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class MiniMap : MonoBehaviour
{
    public GameObject Player;
    public GameObject can;
    public GameObject MiniPLayer;
    public float Altura = 228;
    Quaternion newRotations;
    private void Start()
    {
        MiniPLayer = GameObject.Find("MiniPlayer");
        newRotations.SetEulerAngles(0, 0, 0);
    }

    private void LateUpdate()
    {
        can = GameObject.FindGameObjectWithTag("cine");
        Player = GameObject.Find(PhotonNetwork.NickName);

        Vector3 NewPosition = can.transform.position;
        //NewPosition.z = transform.position.z;
        //NewPosition.x = transform.position.x;
        NewPosition.y = Altura;
        transform.position = NewPosition;



        newRotations.x = Player.transform.rotation.x;
        newRotations.y = Player.transform.rotation.z;
        newRotations.w = Player.transform.rotation.w;
        newRotations.z = Player.transform.rotation.y;


        MiniPLayer.transform.localRotation = newRotations;

    }
}
