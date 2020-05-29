using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndex : MonoBehaviour
{
    public static PlayerIndex instancePlayerIndex;


    public int playerId;
    public List<string> nomes = new List<string>();

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