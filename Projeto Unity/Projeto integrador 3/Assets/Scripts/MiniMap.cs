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
    public Quaternion newRotations;
    public Vector3 vectorRotation;
    private void Start()
    {
        newRotations = new Quaternion(0, 0, 0, 0);
        MiniPLayer = GameObject.Find("MiniPlayer");
    }

    private void LateUpdate()
    {
        can = GameObject.FindGameObjectWithTag("cine");
        Player = GameObject.Find(PhotonNetwork.NickName);

        Vector3 NewPosition = can.transform.position;
        NewPosition.y = Altura;
        transform.position = NewPosition;


        vectorRotation = Vector3.zero;
        vectorRotation.z = Player.transform.rotation.eulerAngles.y * -1;
        vectorRotation.y = Player.transform.rotation.eulerAngles.z;

        newRotations = Quaternion.Euler(vectorRotation);


        MiniPLayer.transform.rotation = newRotations;

    }
}
