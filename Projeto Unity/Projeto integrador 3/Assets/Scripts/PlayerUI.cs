using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    GameObject target;

    public Vector3 offset = new Vector3(0, 0, -2);
    Vector3 TargetPosition;

    public Text Nome;

    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Start()
    {
        Nome = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        
        if(target != null)
        {
            TargetPosition = target.transform.position;
            offset.x = TargetPosition.x;
            offset.z = -2f;
            this.transform.position = Camera.main.ScreenToViewportPoint(TargetPosition) + offset;
            


        }

    }

    public void SetTarget(Player player)
    {
        target = player.gameObject;

        if (Nome != null)
        {
            Nome.text = player.photonView.Owner.NickName;
            print("entrou aqui");
            print(player.photonView.Owner.NickName);
        }

    }

}
