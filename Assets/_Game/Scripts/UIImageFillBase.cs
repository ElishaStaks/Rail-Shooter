using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIImageFillBase
{
    [Tooltip("Image that will be filled")]
    [SerializeField]
    private Image m_ImageToFill;

    public void Initialise()
    {
        if (m_ImageToFill != null) m_ImageToFill.fillAmount = 1;
    }

    public void SetImageFillAmount(float amount)
    {
        m_ImageToFill.fillAmount = amount;
    }
}