using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlVol : MonoBehaviour
{
    public float volumMaster, volumFX, volumMusic;
    public Slider sliderMaster, sliderFX, sliderMusic;

    // Start is called before the first frame update
    void Start()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("Master");
        sliderFX.value = PlayerPrefs.GetFloat("FX");
        sliderMusic.value = PlayerPrefs.GetFloat("Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumMaster(float volum)
    {
        volumMaster = volum;
        AudioListener.volume = volumMaster;

        PlayerPrefs.SetFloat("Master", volumMaster);
    }

    public void VolumFX(float volum)
    {
        volumFX = volum;
        GameObject[] Fxs = GameObject.FindGameObjectsWithTag("Coletavel");

        for(int i = 0; i < Fxs.Length; i++)
        {
            Fxs[i].GetComponent<AudioSource>().volume = volumFX;
        }

        PlayerPrefs.SetFloat("FX", volumFX);
    }

    public void VolumMusic(float volum)
    {
        volumMusic = volum;
        GameObject[] Musics = GameObject.FindGameObjectsWithTag("Music");

        for (int i = 0; i < Musics.Length; i++)
        {
          Musics[i].GetComponent<AudioSource>().volume = volumMusic;
        }

        PlayerPrefs.SetFloat("Music", volumMusic);
    }
}
