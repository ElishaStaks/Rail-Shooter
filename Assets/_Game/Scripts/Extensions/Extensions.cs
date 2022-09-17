using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static float m_NextTimeCanPlayAudio = 0;

    public static void DelayedAction(this MonoBehaviour mb, System.Action action, float delay)
    {
        mb.StartCoroutine(Delayed(action, delay));
    }

    static IEnumerator Delayed(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);

        // If action is not null invoke
        action?.Invoke();
    }

    public static Vector3 GetPositionInsideScreen(Vector2 baseRes, RectTransform rect, float offset)
    {
        float widthBounds = baseRes.x - rect.rect.width - offset;
        float heightBounds = baseRes.y - rect.rect.height - offset;

        Vector2 adjustedPos = rect.anchoredPosition;

        adjustedPos.x = Mathf.Clamp(adjustedPos.x, widthBounds * -0.5f, widthBounds * 0.5f);
        adjustedPos.y = Mathf.Clamp(adjustedPos.y, heightBounds * -0.5f, heightBounds * 0.5f);

        return adjustedPos;
    }

    public static void PlayAudioData(this AudioSource aSource, AudioData audioData)
    {
        aSource.pitch = audioData.GetPitch;
        aSource.clip = audioData.GetAudioClip;
        aSource.volume = audioData.GetVolume;

        if (audioData.GetTimeBetweenPlayedAudio > 0)
        {
            if (Time.time > m_NextTimeCanPlayAudio)
            {
                aSource.Play();
                m_NextTimeCanPlayAudio = Time.time + audioData.GetTimeBetweenPlayedAudio;
            }
        }
        else
        {
            aSource.Play();
        }
    }
}
