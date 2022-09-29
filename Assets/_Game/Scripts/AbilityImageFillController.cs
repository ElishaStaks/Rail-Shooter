using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityImageFillController
{
    [SerializeField]
    private TauntAbility m_Taunt;

    [SerializeField]
    private UIImageFillBase m_ImageFill;

    public void Initialise()
    {
        m_ImageFill.Initialise();

        if (m_Taunt != null)
        {
            TauntAbility.OnTimerChanged += UpdateTimerUI;
        }
        else
        {
            Debug.LogError("No valid player taunt have been referenced!");
        }
    }

    public void RemoveEvents()
    {
        TauntAbility.OnTimerChanged -= UpdateTimerUI;
    }

    private void UpdateTimerUI(float obj)
    {
        if (m_Taunt != null)
        {
            Debug.Log(obj);
            m_ImageFill.SetImageFillAmount(obj / 10);
        }
    }
}
