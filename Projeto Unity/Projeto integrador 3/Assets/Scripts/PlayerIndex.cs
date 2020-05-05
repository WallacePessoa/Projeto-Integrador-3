using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndex : MonoBehaviour
{
    public static PlayerIndex instancePlayerIndex;


    public int playerId;

    void Start()
    {
        if (instancePlayerIndex == null)
        {
            instancePlayerIndex = this;
        }
        else if (instancePlayerIndex != this)
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

    }
}