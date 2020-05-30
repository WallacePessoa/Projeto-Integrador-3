using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class MiniMap : MonoBehaviour
{
    Transform Player;

    private void LateUpdate()
    {
        Player = GameObject.Find(PhotonNetwork.NickName).GetComponent<Transform>();
        Vector3 NewPosition = Player.position;
        NewPosition.z = transform.position.z;
        NewPosition.x = transform.position.x;
        transform.position = NewPosition;
    }
}
