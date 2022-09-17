using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UIManager
{
    public DamageUI m_DamageUI;

    [Header("Health HUD")]

    [SerializeField] 
    private HealthImageFillController m_HealthImageFillController;

    [Header("Ability HUD")]

    [SerializeField] 
    private AbilityImageFillController m_AbilityImageFillController;

    [Header("Weapon HUD")]

    [SerializeField]
    private Image m_WeaponIcon;

    [SerializeField]
    private TextMeshProUGUI m_AmmoText;

    [SerializeField]
    private GameObject m_ReloadWarning;

    [SerializeField]
    private RectTransform m_Crosshair;

    [Space]

    [SerializeField]
    private TextMeshProUGUI m_TimerText;

    [Space]

    [Header("UI Buttons")]

    [SerializeField] 
    private Button m_BackButton;

    [SerializeField]
    private Button m_RestartButton;

    [Space]

    [Header("Scene Names")]

    [SerializeField] 
    private string m_TitleSceneName;

    [SerializeField]
    private string m_MainScene;

    [Space]

    [Header("Stats Properties")]
    [SerializeField]
    private TextMeshProUGUI m_EnemiesKilled;

    [SerializeField]
    private TextMeshProUGUI m_ShotsFired;

    [SerializeField]
    private TextMeshProUGUI m_EnemiesHit;

    [SerializeField]
    private TextMeshProUGUI m_Accuracy;

    [SerializeField]
    private TextMeshProUGUI m_Score;

    [SerializeField]
    private TextMeshProUGUI m_RankText;

    [SerializeField]
    private GameObject m_EndScreenPanel;

    private WeaponData m_CurrentWeaponData;
    private int m_MaxBonusScore = 1000;

    public void Initialise()
    {
        m_HealthImageFillController.Initialise();
        m_AbilityImageFillController.Initialise();

        Weapon.OnWeaponChange += UpdateWeaponUI;

        m_Crosshair.gameObject.SetActive(false);
        if (m_Crosshair != null)
        {
            Cursor.visible = false;
            m_Crosshair.gameObject.SetActive(true);
        }

        if (m_BackButton != null)
            m_BackButton.onClick.AddListener(GoToTitleScene);

        if (m_RestartButton != null)
            m_RestartButton.onClick.AddListener(RestartScene);

        TimerObject.OnTimerChanged += UpdateTimer;
    }

    private void UpdateTimer(int currTimer)
    {
        m_TimerText.SetText(currTimer.ToString("00"));
    }

    private void GoToTitleScene()
    {
        SceneManager.LoadScene(m_TitleSceneName);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(m_MainScene);
    }

    public void RemoveEvents()
    {
        m_AbilityImageFillController.RemoveEvents();
        Weapon.OnWeaponChange -= UpdateWeaponUI;
        m_CurrentWeaponData.OnWeaponShot -= UpdateAmmoUI;
        TimerObject.OnTimerChanged -= UpdateTimer;
    }

    private void UpdateWeaponUI(WeaponData obj)
    {
        if (m_CurrentWeaponData != null)
        {
            m_CurrentWeaponData.OnWeaponShot -= UpdateAmmoUI;
        }

        m_CurrentWeaponData = obj;
        m_CurrentWeaponData.OnWeaponShot += UpdateAmmoUI;
        m_WeaponIcon.sprite = m_CurrentWeaponData.GetWeaponIcon;
    }

    private void UpdateAmmoUI(int ammo)
    {
        m_ReloadWarning.SetActive(ammo <= 0);
        m_AmmoText.SetText(ammo.ToString("00"));
    }

    public void MoveCrosshair(Vector3 mousePos)
    {
        if (m_Crosshair != null)
        {
            m_Crosshair.position = mousePos;
        }
    }

    public void ShowEndScreen(int enemiesKilled, int totalEnemies, int shotsFired, int totalHits, int score)
    {
        m_EndScreenPanel.SetActive(true);
        m_EnemiesKilled.SetText(((enemiesKilled / (float)totalEnemies) * 100f).ToString("00") + "%");
        m_ShotsFired.SetText(shotsFired.ToString());
        m_EnemiesHit.SetText(totalHits.ToString());
        m_Accuracy.SetText(((totalHits / (float)shotsFired) * 100f).ToString("00") + "%");

        float accuracy = (totalHits / (float)shotsFired) * 100f;

        Debug.Log(accuracy);

        if (accuracy > 40)
        {
            m_Score.SetText(((int)accuracy * m_MaxBonusScore + score).ToString());
        }
        else
        {
           m_Score.SetText(score.ToString());
        }
        CalculateRank(enemiesKilled, totalEnemies, shotsFired, totalHits, score);
    }

    public void CalculateRank(int enemiesKilled, int totalEnemies, int shotsFired, int totalHits, int score)
    {
        float enemiesKilledRatio = ((enemiesKilled / (float)totalEnemies) * 100f) + score;
        float accuracyRatio = ((totalHits / (float)shotsFired) * 100f) + score;
        float totalAverage = (enemiesKilledRatio + accuracyRatio) / 2f;

        if (totalAverage >= 85f)
        {
            m_RankText.SetText("A");
        }
        else if (totalAverage >= 72f && totalAverage < 85f)
        {
            m_RankText.SetText("B");
        }
        else if (totalAverage >= 57f && totalAverage < 72f)
        {
            m_RankText.SetText("C");
        }
        else if (totalAverage < 57f)
        {
            m_RankText.SetText("D");
        }
    }
}
