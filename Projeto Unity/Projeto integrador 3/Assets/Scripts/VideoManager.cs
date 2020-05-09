using Microsoft.Azure.Amqp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LightWeightPipeline;
using UnityEngine.Rendering;


public class VideoManager : Singleton<VideoManager>
{
    public LightWeightRendererPipelineAsset[] quality;

    public static void ChangeQualitySettings(int value)
    {
        //QualitySettings.SetQualitySettings(value, true);
        GraphicsSettings.renderPipelineAsset = VideoManager.Instance.quality[value];
        PlayerPrefs.SetInt("QualitySettings", value);
    }

    public static void ChangeResolutionSettings(int value)
    {
        PlayerPrefs.SetInt("ResolutionSettings", value);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("QualitySettings"))
        {
            ChangeQualitySettings(PlayerPrefs.GetInt("QualitySettings"));
        }

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
