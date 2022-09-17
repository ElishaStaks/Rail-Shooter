using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    [SerializeField] 
    private string m_AudioName;

    [SerializeField] 
    private AudioClip[] m_AudioClips;

    [SerializeField]
    [Range(0, 1)]
    private float m_Volume = 0.5f;

    [SerializeField] 
    private float m_MinPitch = 0.9f;

    [SerializeField]
    private float m_MaxPitch = 1.1f;

    [SerializeField]
    private float m_TimeBetweenPlayedAudio = 0f;

    public string AudioName { get => m_AudioName; }
    public AudioClip GetAudioClip { get => m_AudioClips[Random.Range(0, m_AudioClips.Length)]; }
    public float GetPitch { get => Random.Range(m_MinPitch, m_MaxPitch); }

    public float GetVolume { get => m_Volume; }
    
    public float GetTimeBetweenPlayedAudio { get => m_TimeBetweenPlayedAudio;}
}
