using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : Singleton<AudioPlayer>
{
    [SerializeField] 
    private AudioLibrary m_AudioLib;

    [SerializeField] 
    private int m_AudioSourcesNumber = 6;

    [SerializeField] 
    private AudioMixerGroup m_SfxGroup;

    [SerializeField]
    private AudioMixerGroup m_BgmGroup;

    private Queue<AudioSource> m_AudioSources = new Queue<AudioSource>();
    private AudioSource m_BgmSource;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    void Initialise()
    {
        GameObject bgmObject = new GameObject("BGM Source");
        m_BgmSource = bgmObject.AddComponent<AudioSource>();
        m_BgmSource.spatialBlend = 0f;
        m_BgmSource.outputAudioMixerGroup = m_BgmGroup;
        bgmObject.transform.SetParent(transform);

        for (int i = 0; i < m_AudioSourcesNumber; i++)
        {
            GameObject sfxObject = new GameObject("SFX Source " + (i + 1).ToString("00"));
            AudioSource temp = sfxObject.AddComponent<AudioSource>();
            temp.spatialBlend = 1f;
            temp.outputAudioMixerGroup = m_SfxGroup;
            sfxObject.transform.SetParent(transform);
            m_AudioSources.Enqueue(temp);
        }
    }

    public void PlaySFX(AudioGetter audioSfx, Transform audioLocation = null)
    {
        AudioSource temp = m_AudioSources.Dequeue();
        if (audioLocation != null)
        {
            temp.transform.position = audioLocation.position;
            temp.spatialBlend = 1f;
        }
        else
        {
            temp.spatialBlend = 0f;
        }

        temp.PlayAudioData(m_AudioLib.GetAudioByName(audioSfx.AudioName));
        m_AudioSources.Enqueue(temp);
    }

    public void PlayMusic(AudioGetter music)
    {
        m_BgmSource.PlayAudioData(m_AudioLib.GetAudioByName(music.AudioName));
        m_BgmSource.pitch = 1f;
    }
}
