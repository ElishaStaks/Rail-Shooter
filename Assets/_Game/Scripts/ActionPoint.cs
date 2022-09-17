using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionPoint : MonoBehaviour
{
    public bool AreaCleared { get; private set; }
    public CameraPathMovement CameraPathMovement { get { return m_CameraPathMovement; } }

    [SerializeField]
    private EnemyEntry[] m_EnemyEntries;

    private bool m_ActivePoint = false;
    private CameraPathMovement m_CameraPathMovement;
    private int m_EnemiesKilled = 0;

    public CameraPathMovement Initialise(CameraPathMovement cameraPathMovement)
    {
        m_CameraPathMovement = cameraPathMovement;
        return m_CameraPathMovement;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CameraReachedPoint(float timer)
    {
        m_ActivePoint = true;
        m_CameraPathMovement.SetCameraMovement(false);
        StartCoroutine(SpawnEnemy());
        this.DelayedAction(SetAreaCleared, timer);
        GameManager.Instance.StartTimer(timer);
    }

    IEnumerator SpawnEnemy()
    {
        foreach (var enemyEntry in m_EnemyEntries)
        {
            yield return new WaitForSeconds(enemyEntry.spawnDelay);

            enemyEntry.enemy.Initialise(this);

            Debug.Log(enemyEntry.enemy.gameObject.name + " has spawned!");
        }
    }

    public void EnemyKilled()
    {
        m_EnemiesKilled++;

        if (m_EnemiesKilled == m_EnemyEntries.Length)
        {
            m_CameraPathMovement.AreaCleared();
            AreaCleared = true;
            m_ActivePoint = false;
            GameManager.Instance.StopTimer();
        }
    }

    public void SetAreaCleared()
    {
        if (AreaCleared)
            return;

        AreaCleared = true;
        m_CameraPathMovement.AreaCleared();
        foreach (var enemy in m_EnemyEntries)
        {
            if (enemy.enemy == null)
                continue;

            enemy.enemy.StopShooting();
        }
    }
}

[System.Serializable]
public class EnemyEntry
{
    public Enemy enemy;
    public float spawnDelay;
}
