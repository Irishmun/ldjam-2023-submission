using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BackgroundSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider AmbienceSlider;
    private void Awake()
    {
        sensitivitySlider.value = GlobalState.SENSITIVITY;
        mixer.GetFloat("Master", out float val);
        MasterSlider.value = val;
        mixer.GetFloat("Background Music", out val);
        BackgroundSlider.value = val;
        mixer.GetFloat("SFX", out val);
        SFXSlider.value = val;
        mixer.GetFloat("Ambience", out val);
        AmbienceSlider.value = val;
    }
    public void SetSensitivity(Single value)
    {
        GlobalState.SENSITIVITY = value;
    }

    public void SetMasterVolume(Single value)
    {
        mixer.SetFloat("Master", value);
    }

    public void SetBackgroundVolume(Single value)
    {
        mixer.SetFloat("Background Music", value);
    }

    public void SetSFXVolume(Single value)
    {
        mixer.SetFloat("SFX", value);
    }

    public void SetAmbienceVolume(Single value)
    {
        mixer.SetFloat("Ambience", value);
    }

}
