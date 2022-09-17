using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateEnabler : MonoBehaviour
{
    [SerializeField]
    private List<GameState> m_gameStates = new List<GameState>();

    public UnityEvent onGameStateEntered;
    public UnityEvent onGameStateExited;

    protected virtual void Awake()
    {
        if (GameStateManager.Instance != null)
        {
            List<StateInstance> gameStates = GameStateManager.Instance.GameStates;

            for (int i = 0; i < gameStates.Count; i++)
            {
                gameStates[i].onEnteredGameState.AddListener(OnEnteredGameState);
            }
        }
    }

    protected virtual void OnEnteredGameState(GameState state)
    {
        if (m_gameStates.IndexOf(state) != -1)
        {
            onGameStateEntered.Invoke();
        }
        else
        {
            onGameStateExited.Invoke();
        }
    }
}

