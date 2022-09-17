using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerObject
{
    public static System.Action<int> OnTimerChanged = delegate { };

    public int displayTimer;
    private Coroutine m_Timer;

    public void StartTimer(MonoBehaviour mb, float duration)
    {
        if (m_Timer != null)
        {
            Debug.Log("Timer already runs");
            return;
        }

        m_Timer = mb.StartCoroutine(TimerRuns(duration));
    }

    public void StopTimer(MonoBehaviour mb)
    {
        if (m_Timer == null)
        {
            Debug.Log("There are no timer currently running");
            return;
        }

        mb.StopCoroutine(m_Timer);
        m_Timer = null;
    }

    IEnumerator TimerRuns(float duration)
    {
        while(duration > 0f)
        {
            OnTimerChanged((int)duration);
            displayTimer = (int)duration;
            duration -= 1f;
            yield return new WaitForSeconds(1f);
        }

        m_Timer = null;
    }
}
