using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnTauntEvent : UnityEvent { }

[System.Serializable]
public class OnFinishTauntEvent : UnityEvent { }

public class TauntAbility : MonoBehaviour
{
    public static System.Action<float> OnTimerChanged = delegate { }; 

    [SerializeField]
    private float m_TauntTime;

    [SerializeField]
    private AudioGetter m_TauntSfx;

    [SerializeField]
    private AudioGetter m_TauntIntroMusic;

    private bool m_IsTaunting = false;
    private bool m_CanTaunt = true;

    private float m_InitalTauntTime;

    public OnTauntEvent onTaunt;
    public OnFinishTauntEvent onFinishTaunt;

    private void Start()
    {
        m_InitalTauntTime = m_TauntTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !m_IsTaunting && m_CanTaunt)
        {
            // TODO: Play some audio clips / sound effects
            AudioPlayer.Instance.PlayMusic(m_TauntIntroMusic);
            AudioPlayer.Instance.PlaySFX(m_TauntSfx, transform);
            m_IsTaunting = true;
            m_CanTaunt = false;
            StartCoroutine(DoTaunt(m_TauntTime));
            onTaunt.Invoke();
        }
    }

    public void AddTauntScore()
    {
        if (m_TauntTime == m_InitalTauntTime) return;

        m_TauntTime += 0.5f;
        OnTimerChanged((float)m_TauntTime);

        if (m_TauntTime == m_InitalTauntTime)
        {
            m_CanTaunt = true;
        }
    }

    IEnumerator DoTaunt(float timer)
    {
        yield return new WaitForSeconds(0.2f);

        while (timer > 0f)
        {
            OnTimerChanged((float)timer);
            timer -= Time.deltaTime;
            yield return null;
        }

        m_IsTaunting = false;
        m_TauntTime = 0f;
        onFinishTaunt.Invoke();
    }
}
