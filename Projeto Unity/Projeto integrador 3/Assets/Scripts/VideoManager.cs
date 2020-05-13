using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ChangeResolutionSettings(int value)
    {
        PlayerPrefs.SetInt("ResolutionSettings", value);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("ResolutionSettings"))
        {
            ChangeResolutionSettings(PlayerPrefs.GetInt("ResolutionSettings"));
        }

        else
        {
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if(Screen.resolutions[i].height == Screen.currentResolution.height && Screen.resolutions[i].width == Screen.currentResolution.width)
                {
                    ChangeResolutionSettings(i);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
