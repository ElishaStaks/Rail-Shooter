using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicAction : MonoBehaviour
{
    [SerializeField] 
    private AudioGetter m_AudioSfx;

    [SerializeField]
    private float m_Delay;

    private void OnEnable()
    {
        this.DelayedAction(delegate
        {
            AudioPlayer.Instance.PlayMusic(m_AudioSfx);
        }, m_Delay);
    }
}
