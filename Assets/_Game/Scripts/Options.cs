using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Options : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Dropdown m_ResolutionDropDown;

    [SerializeField]
    private TMPro.TMP_Dropdown m_QualityDropDown;

    private Resolution[] m_Resolutions;

    // Start is called before the first frame update
    void Start()
    {
        m_Resolutions = Screen.resolutions;

        List<string> options = new List<string>();

        int currentScreenResolutionId = 0;

        for (int i = 0; i < m_Resolutions.Length; i++)
        {
            string res = m_Resolutions[i].width + " x " + m_Resolutions[i].width;
            options.Add(res);

            if (Screen.currentResolution.width == m_Resolutions[i].width 
                && Screen.currentResolution.height == m_Resolutions[i].height)
            {
                currentScreenResolutionId = i;
            }
        }

        m_ResolutionDropDown.ClearOptions();
        m_ResolutionDropDown.AddOptions(options);
        m_ResolutionDropDown.value = currentScreenResolutionId;

        m_QualityDropDown.ClearOptions();
        m_QualityDropDown.AddOptions(QualitySettings.names.ToList());
        m_QualityDropDown.value = QualitySettings.GetQualityLevel();

        m_QualityDropDown.onValueChanged.AddListener(SetQuality);
        m_ResolutionDropDown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionId)
    {
        Resolution res = m_Resolutions[resolutionId];

        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityId)
    {
        QualitySettings.SetQualityLevel(qualityId);
    }
}
