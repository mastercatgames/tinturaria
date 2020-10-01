using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    void Start()
    {
        if (PlayerPrefs.GetString("music") == "")
        {
            PlayerPrefs.SetString("music", "on");
        }
        if (PlayerPrefs.GetString("sfx") == "")
        {
            PlayerPrefs.SetString("sfx", "on");
        }

        SetMusicLvl(PlayerPrefs.GetString("music") == "on" ? true : false);
        SetSFXLvl(PlayerPrefs.GetString("sfx") == "on" ? true : false);
    }

    public void SetMusicLvl(bool isOn)
    {
        PlayerPrefs.SetString("music", isOn ? "on" : "off");
        musicToggle.isOn = isOn;

        if (isOn)
            audioMixerGroup.audioMixer.SetFloat("MusicVol", 0f);
        else
            audioMixerGroup.audioMixer.SetFloat("MusicVol", -80f);
    }

    public void SetSFXLvl(bool isOn)
    {
        PlayerPrefs.SetString("sfx", isOn ? "on" : "off");
        sfxToggle.isOn = isOn;

        if (isOn)
            audioMixerGroup.audioMixer.SetFloat("SFXVol", 0f);
        else
            audioMixerGroup.audioMixer.SetFloat("SFXVol", -80f);
    }

    public void PlaySFX(string SFXName)
    {
        transform.Find("SFX").Find(SFXName).GetComponent<AudioSource>().Play();
    }

    public bool AudioIsPlaying(string SFXName)
    {
        return transform.Find("SFX").Find(SFXName).GetComponent<AudioSource>().isPlaying;
    }
}
