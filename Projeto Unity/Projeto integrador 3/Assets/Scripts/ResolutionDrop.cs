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

        //for(int i = 0; i < Screen.resolutions.Lenght; i++)
        //{
        //    Dropdown.OptionData item = new Dropdown.OptionData();
        //    item.text = Screen.resolutions[i].widht + " x " + Screen.resolutions[i] + " (" + Screen.resolution[i].refreshRate + ")";
        //    options.Add(item);
        //}

        dropdown.AddOptions(options);
        //dropdown.value = PlayerPrefs.GetInt("ResolutionSetting", Screen.resolutions.Lengh);
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
