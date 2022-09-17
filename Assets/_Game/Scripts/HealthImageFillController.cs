using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthImageFillController
{
    public Damagable PlayerVitals { get { return m_PlayerVitals; } }

    [Tooltip("Reference to the players damagable component")]
    [SerializeField]
    private Damagable m_PlayerVitals;

    [SerializeField]
    private UIImageFillBase m_ImageFill;

    public void Initialise()
    {
        m_ImageFill.Initialise();

        if (m_PlayerVitals != null)
        {
            m_PlayerVitals.onDamaged.AddListener(VitalUI);
            m_PlayerVitals.onHealed.AddListener(VitalUI);
        }
        else
        {
            Debug.LogError("No valid player vitals have been referenced!");
        }
    }

    public void VitalUI()
    {
        if (m_PlayerVitals != null)
        {
            m_ImageFill.SetImageFillAmount(m_PlayerVitals.CurrentHealth / m_PlayerVitals.MaxHealth);
        }
    }
}
