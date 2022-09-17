using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private UIManager m_UIManager = new UIManager();
    
    [Space]

    [SerializeField]
    private VolumeSettings m_VolumeSettings = new VolumeSettings();

    [Space]

    [Tooltip("Strictly should be the pause state")]
    [SerializeField]
    private GameState m_PauseGameState;

    [Tooltip("Strictly should be the resume state")]
    [SerializeField]
    private GameState m_ResumeGameState;


    private TimerObject m_TimerObject = new TimerObject();
    private bool m_IsPaused = false;
    private int m_EnemiesHit;
    private int m_EnemiesKilled;
    private int m_TotalEnemies;
    private int m_ShotsFired;
    private int m_Score;

    private void OnDisable()
    {
        CameraPathMovement.OnLevelFinished -= ShowEndScreen;
        m_UIManager.RemoveEvents();
    }

    private void Start()
    {
        Initialise();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.Instance.CurrentGameState.name != "Dead")
        {
            if (GameStateManager.Instance != null)
            {
                m_IsPaused = !m_IsPaused;

                if (m_IsPaused)
                {
                    GameStateManager.Instance.EnterGameState(m_PauseGameState);
                }
                else
                {
                    GameStateManager.Instance.EnterGameState(m_ResumeGameState);
                }
            }
        }

        m_UIManager.MoveCrosshair(Input.mousePosition);
    }

    private void Initialise()
    {
        m_UIManager.Initialise();
        m_VolumeSettings.Initialise();
        CameraPathMovement.OnLevelFinished += ShowEndScreen;
    }

    public void BloodScreen()
    {
        m_UIManager.m_DamageUI.OnHealthChanged(this);
    }

    public void EnemiesHit()
    {
        m_EnemiesHit++;
    }

    public void ShotFired()
    {
        m_ShotsFired++;
    }

    public void Delay()
    {
        this.DelayedAction(delegate { GameStateManager.Instance.EnterGameState(m_ResumeGameState); }, 2f);
    }

    public void StartGame(int index = -1)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Load the game scene
        if (index == -1)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void StartTimer(float duration)
    {
        m_TimerObject.StartTimer(this, duration);
    }

    public void StopTimer()
    {
        m_TimerObject.StopTimer(this);
    }

    public void RegisterEnemy()
    {
        m_TotalEnemies++;
    }

    public void EnemyKilled()
    {
        m_EnemiesKilled++;
    }

    public void AddScore(int amount)
    {
        m_Score += amount;
    }

    private void ShowEndScreen()
    {
        this.DelayedAction(delegate { m_UIManager.ShowEndScreen(m_EnemiesKilled, m_TotalEnemies, m_ShotsFired, m_EnemiesHit, m_Score); }, 0.2f);
    }
}
