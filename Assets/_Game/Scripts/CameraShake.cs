using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Kino;

public class CameraShake : Singleton<CameraShake>
{
    [SerializeField]
    private AnimationCurve m_GlitchCurve;

    //private DigitalGlitch m_GlitchFX;

    private void Start()
    {
        //m_GlitchFX = GetComponent<DigitalGlitch>();
    }

    IEnumerator DoCameraShake(float timer, float amp, float freq)
    {
        Vector3 initialPos = transform.position;
        Vector3 newPos = transform.position;
        float duration = timer;

        yield return new WaitForSeconds(0.2f);

        while (duration > 0f)
        {
            if ((newPos - transform.position).sqrMagnitude < 0.01f)
            {
                newPos = initialPos;
                newPos.x += Random.Range(-1f, 1f) * amp;
                newPos.y += Random.Range(1f, 1f) * amp;
            }

            transform.position = Vector3.Lerp(transform.position, newPos, freq * Time.deltaTime);

            //m_GlitchFX.intensity = m_GlitchCurve.Evaluate(duration / timer);

            duration -= Time.deltaTime;
            yield return null;
        }

        //m_GlitchFX.intensity = 0.0f;
        transform.position = initialPos;
    }

    public void ShakeCamera(float timer, float amp, float freq)
    {
        StartCoroutine(DoCameraShake(timer, amp, freq));
    }
}
