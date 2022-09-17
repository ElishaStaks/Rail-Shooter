using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public class VolumeSettings
{
    public GameObject Panel { get => m_Panel; }

    [SerializeField] 
    private GameObject m_Panel;

    [SerializeField] 
    private Slider m_SfxSlider, m_BgmSlider;

    [SerializeField] 
    private AudioMixer m_MasterMixer;

    private string m_SfxKey = "SfxKey" , m_BgmKey = "BgmKey";

    public void Initialise()
    {
        AdjustSfx(PlayerPrefs.GetFloat(m_SfxKey, 0f));
        AdjustBgm(PlayerPrefs.GetFloat(m_BgmKey, 0f));

        m_SfxSlider.value = PlayerPrefs.GetFloat(m_SfxKey, 0f);
        m_BgmSlider.value = PlayerPrefs.GetFloat(m_BgmKey, 0f);

        m_SfxSlider.onValueChanged.AddListener(AdjustSfx);
        m_BgmSlider.onValueChanged.AddListener(AdjustBgm);
    }

    void AdjustSfx(float value)
    {
        m_MasterMixer.SetFloat("SfxVolume", value);
        PlayerPrefs.SetFloat(m_SfxKey, value);
        PlayerPrefs.Save();
    }

    void AdjustBgm(float value)
    {
        m_MasterMixer.SetFloat("BackgroundVolume", value);
        PlayerPrefs.SetFloat(m_BgmKey, value);
        PlayerPrefs.Save();
    }
}