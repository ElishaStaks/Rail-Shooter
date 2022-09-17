using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// Unity event for running functions when the game state changes.
/// </summary>
[System.Serializable]
public class OnEnteredGameStateEventHandler : UnityEvent<GameState>
{

}

/// <summary>
/// Provides a way for the user to set parameters for each of the game states.
/// </summary>
[System.Serializable]
public class StateInstance
{
    public GameState gameState;
    public bool freezeTimeOnEntry;
    public float freezeBeforeEntry = 0f;
    public bool restrictEntryStates;
    public bool showCursor = true;
    public bool centerCursorAtStart = true;
    public bool lockCursor = false;
    public List<GameState> allowedEntryStates = new List<GameState>();

    [Header("Events")]
    [Tooltip("Event for running functions when this game state changes")]
    public OnEnteredGameStateEventHandler onEnteredGameState;
}

/// <summary>
/// Managers the current state of the game.
/// </summary>
public class GameStateManager : Singleton<GameStateManager>
{
    public GameState CurrentGameState { get { return m_currentGameState; } }
    public List<StateInstance> GameStates { get { return m_gameStates; } }

    [Tooltip("The starting game state")]
    [SerializeField]
    private GameState m_startingGameState;

    [Tooltip("A list that stores the parameters associated with each game state")]
    [SerializeField]
    private List<StateInstance> m_gameStates = new List<StateInstance>();

    // Enter game state with a delay using a coroutine
    private Coroutine m_enterGameStateCoroutine;
    private bool m_enteringState = false;
    private GameState m_currentGameState;

    protected void Awake()
    {
        // Add the function to be called when the scene is exited
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected virtual void Start()
    {
        if (m_startingGameState != null)
        {
            EnterGameState(m_startingGameState);
        }
        else
        {
            Debug.LogError("Oops! No starting state has been assigned...assign me a state and I won't complain.");
        }
    }

    /// <summary>
    /// Called when the scene manager exits a scene. Disable any cursor lock and show cursor.
    /// </summary>
    /// <param name="scene"></param>
    private void OnSceneUnloaded(Scene scene)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    private IEnumerator FreezeBeforeEntryCoroutine(float freezeTime, StateInstance nextState)
    {
        m_enteringState = true;
        yield return new WaitForSeconds(freezeTime);
        m_enteringState = false;
        InvokeGameState(nextState);
    }

    private void InvokeGameState(StateInstance newStateInstance)
    {
        if (m_enteringState)
        {
            return;
        }

        // Update the current game state to the new game state
        m_currentGameState = newStateInstance.gameState;

        // Stop coroutine
        if (m_enterGameStateCoroutine != null)
        {
            StopCoroutine(m_enterGameStateCoroutine);
        }

        // Freeze time if we are told to
        if (newStateInstance.freezeTimeOnEntry)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }

        SetCursorVisible(newStateInstance.showCursor);

        if (newStateInstance.centerCursorAtStart)
        {
            CenterCursor();
        }

        SetCursorLock(newStateInstance.lockCursor);

        // Call event
        newStateInstance.onEnteredGameState.Invoke(m_currentGameState);
    }

    public void EnterGameState(GameState newState)
    {
        if (m_enteringState)
        {
            return;
        }

        for (int i = 0; i < m_gameStates.Count; ++i)
        {
            if (m_gameStates[i].gameState == newState)
            {
                if (m_gameStates[i].restrictEntryStates)
                {
                    bool allow = false;
                    foreach (GameState allowedOriginState in m_gameStates[i].allowedEntryStates)
                    {
                        if (m_currentGameState == allowedOriginState)
                        {
                            allow = true;
                            break;
                        }
                    }

                    if (!allow) return;
                }

                if (!Mathf.Approximately(m_gameStates[i].freezeBeforeEntry, 0))
                {
                    m_enterGameStateCoroutine = StartCoroutine(FreezeBeforeEntryCoroutine(m_gameStates[i].freezeBeforeEntry, m_gameStates[i]));
                }
                else
                {
                    InvokeGameState(m_gameStates[i]);
                }
            }
        }
    }

    /// <summary>
    /// Center the cursor
    /// </summary>
    public void CenterCursor()
    {
        CursorLockMode initialState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = initialState;
    }

    /// <summary>
    /// Change cursor visibility.
    /// </summary>
	public void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }

    /// <summary>
    /// Set the cursor locked or not.
    /// </summary>
    /// <param name="locked">Whether to lock the cursor.</param>
    public void SetCursorLock(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
