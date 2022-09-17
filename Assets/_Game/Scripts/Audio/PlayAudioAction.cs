using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioAction : MonoBehaviour
{
    [SerializeField] 
    private AudioGetter m_AudioSfx;

    [SerializeField] 
    private bool m_2DSound;

    [SerializeField] 
    private float m_Delay;

    private void OnEnable()
    {
        this.DelayedAction(delegate
        {
            AudioPlayer.Instance.PlaySFX(m_AudioSfx, m_2DSound ? null : transform);
        }, m_Delay);
    }
}
