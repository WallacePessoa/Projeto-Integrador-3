using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public Vector3 offSet;
    GameObject Target;
    public TextMeshPro Text;

    private void Awake()
    {
        Text = GetComponent<TextMeshPro>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Target != null)
            transform.position = Target.transform.position + offSet;
    }
    public void SetTarget(Player player)
    {
        Target = player.gameObject;

        if(Text != null)
        {
            Text.text = player.photonView.Owner.NickName;
            print("entrou aqui");
            print(player.photonView.Owner.NickName);

        }



    }
}
