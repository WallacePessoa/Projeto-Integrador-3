using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlVol : MonoBehaviour
{
    public float volumMaster, volumFX, volumMusic;
    public Slider sliderMaster, sliderMusic, sliderFX;

    // Start is called before the first frame update
    void Start()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("Master");        
        sliderMusic.value = PlayerPrefs.GetFloat("Music");
        sliderFX.value = PlayerPrefs.GetFloat("FX");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumMaster(float volume)
    {
        volumMaster = volume;
        AudioListener.volume = volumMaster;

        PlayerPrefs.SetFloat("Master", volumMaster);
    }
    public void VolumMusic(float volume)
    {
        volumMusic = volume;
        GameObject[] Musics = GameObject.FindGameObjectsWithTag("Music");

        for (int i = 0; i < Musics.Length; i++)
        {
            Musics[i].GetComponent<AudioSource>().volume = volumMusic;
        }

        PlayerPrefs.SetFloat("Music", volumMusic);
    }

    public void VolumFX(float volume)
    {
        volumFX = volume;
        GameObject[] Fxs = GameObject.FindGameObjectsWithTag("FX");

        for(int i = 0; i < Fxs.Length; i++)
        {
            Fxs[i].GetComponent<AudioSource>().volume = volumFX;
        }

        PlayerPrefs.SetFloat("FX", volumFX);
    }   
}
