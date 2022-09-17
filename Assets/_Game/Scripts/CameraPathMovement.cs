using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class CameraPathMovement : MonoBehaviour
{
    public ActionPointEntry CurrentActionPoint { get => m_CurrentActionPointEntry; }
    public static System.Action OnLevelFinished = delegate { };

    [SerializeField]
    private PathCreator m_MainPath;

    [SerializeField]
    private EndOfPathInstruction m_EndOfPathInstruction;

    [SerializeField]
    private float m_Speed = 3f;

    [SerializeField]
    private bool m_IsMoving = false;

    [SerializeField]
    private ActionPointEntry[] m_ActionPointEntries;

    [Header("Debug Options")]
    [SerializeField]
    private float m_PreviewDistance = 0f;

    [SerializeField]
    private bool m_EnableDebug = false;

    private float m_DistanceTravelled;
    private int areaCleared;
    private Vector3 m_LatestActionPointPosition;
    private ActionPointEntry m_CurrentActionPointEntry;
    private Vector3 v = Vector3.zero;

    private void Start()
    {
        foreach (var entry in m_ActionPointEntries)
        {
           entry.actionPoint.Initialise(this);
        }

        SetCameraMovement(true);
    }

    private void Update()
    {
        MoveCamera();
        HandleCoverInput(m_CurrentActionPointEntry);
    }

    private void MoveToCover(Vector3 position)
    {
        m_CurrentActionPointEntry.inCover = true;
        transform.position = Vector3.Lerp(transform.position, position, 10f * Time.deltaTime);

        var a = Vector3.Distance(transform.position, position);

        if (a < 0.001f)
        {
            transform.position = position;
        }
    }

    private void HandleCoverInput(ActionPointEntry actionPointEntry)
    {
        if (!m_IsMoving)
        {
            if (Input.GetKey(KeyCode.A) && actionPointEntry.coverData.left != null)
            {
                MoveToCover(actionPointEntry.coverData.left.position);
            }
            else if (Input.GetKey(KeyCode.D) && actionPointEntry.coverData.right != null)
            {
                MoveToCover(actionPointEntry.coverData.right.position);
            }
            else if (Input.GetKey(KeyCode.S) && actionPointEntry.coverData.down != null)
            {
                MoveToCover(actionPointEntry.coverData.down.position);
            }
            else
            {
                actionPointEntry.inCover = false;
                transform.position = Vector3.Lerp(transform.position, m_LatestActionPointPosition, 10f * Time.deltaTime);

                var a = Vector3.Distance(transform.position, m_LatestActionPointPosition);

                if (a < 0.001f)
                {
                    transform.position = m_LatestActionPointPosition;
                }
                return;
            }
        }
    }

    private void MoveCamera()
    {
        if (m_MainPath != null && m_IsMoving)
        {
            m_DistanceTravelled += m_Speed * Time.deltaTime;
            transform.position = m_MainPath.path.GetPointAtDistance(m_DistanceTravelled, m_EndOfPathInstruction);
            transform.rotation = m_MainPath.path.GetRotationAtDistance(m_DistanceTravelled, m_EndOfPathInstruction);

            for (int i = 0; i < m_ActionPointEntries.Length; i++)
            {
                if ((m_MainPath.path.GetPointAtDistance(m_ActionPointEntries[i].distance) - transform.position).sqrMagnitude < 0.001f)
                {
                    m_LatestActionPointPosition = m_MainPath.path.GetPointAtDistance(m_ActionPointEntries[i].distance);
                    m_CurrentActionPointEntry = m_ActionPointEntries[i];

                    if (m_ActionPointEntries[i].actionPoint.AreaCleared)
                    {
                        return;
                    }

                    if (m_IsMoving)
                    {
                        m_ActionPointEntries[i].actionPoint.CameraReachedPoint(m_ActionPointEntries[i].areaTimer);
                    }
                }
            }
        }
    }

    public void SetCameraMovement(bool isEnable)
    {
        m_IsMoving = isEnable;
    }

    public void AreaCleared()
    {
        areaCleared++;

        if (areaCleared == m_ActionPointEntries.Length)
        {
            OnLevelFinished();
            return;
        }

        SetCameraMovement(true);
    }

    /// <summary>
    /// Whenever we change a value in inspector this method gets executed.
    /// </summary>
    private void OnValidate()
    {
        if (m_EnableDebug)
        {
            transform.position = m_MainPath.path.GetPointAtDistance(m_PreviewDistance, m_EndOfPathInstruction);
            transform.rotation = m_MainPath.path.GetRotationAtDistance(m_PreviewDistance, m_EndOfPathInstruction);
        }
    }
}

/// <summary>
/// Event for when the entity is dead
/// </summary>
[System.Serializable]
public class OnActionPointBegin : UnityEvent { }

/// <summary>
/// Event for when the entity is dead
/// </summary>
[System.Serializable]
public class OnActionPointComplete : UnityEvent { }


[System.Serializable]
public struct CoverData {
    public Transform left;
    public Transform right;
    public Transform down;
}

[System.Serializable]
 public class ActionPointEntry
{
    public ActionPoint actionPoint;
    public bool inCover;
    public CoverData coverData;
    public float distance;
    public float areaTimer = 15f;
    public OnActionPointBegin onActionPointBegin;
    public OnActionPointBegin onActionPointComplete;
}
