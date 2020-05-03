using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDrop : MonoBehaviour
{
    Dropdown dropdown;
    List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("UnitySelectMonitor", 0);

        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();

        //for(int i = 0; i < Screen.
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
