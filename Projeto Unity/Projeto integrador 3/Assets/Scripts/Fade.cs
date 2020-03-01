using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public GameObject go1;
    public GameObject go2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }
    IEnumerator FadeOut()
    {
        for(float x = 1; x > 0; x -= 0.2f)
        {
            Color c1 = go1.GetComponent<Renderer>().material.color;
            c1.a = x;
            go1.GetComponent<Renderer>().material.color = c1;

            Color c2 = go2.GetComponent<Renderer>().material.color;
            c2.a = x;
            go2.GetComponent<Renderer>().material.color = c2;

            yield return null;
        }
    }
}
