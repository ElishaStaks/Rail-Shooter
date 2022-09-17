using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float DamageAmount { get { return m_DamageAmount; } }

    [SerializeField]
    private Transform m_TargetPosition;

    [Header("Shoot Properties")]
    [SerializeField]
    private float m_DamageAmount = 5f;

    [SerializeField]
    private IntervalRange m_Interval = new IntervalRange(1.5F, 2.7f);

    [SerializeField]
    private float m_ShootAccuracy = 0.5f;

    [SerializeField]
    private ParticleSystem m_DamageShotFx;

    [SerializeField]
    private ParticleSystem m_Shotfx;

    private List<ParticleCollisionEvent> m_particleCollisionEvents;
    private Transform m_Player;
    private NavMeshAgent m_Agent;
    private ActionPoint m_ActionPoint;
    private Animator m_Anim;
    private Vector3 m_LocalMovement;
    private Damagable m_Damagable;

    // Start is called before the first frame update
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = Camera.main.transform;
        m_Anim = GetComponentInChildren<Animator>();
        m_Damagable = GetComponent<Damagable>();

        // Ignore rotation
        m_Agent.updateRotation = false;
        m_Agent.updatePosition = true;
        gameObject.SetActive(false);
    }

    public void Initialise(ActionPoint point)
    {
        gameObject.SetActive(true);
        m_ActionPoint = point;

        if (m_Agent != null)
        {
            m_Agent.SetDestination(m_TargetPosition.position);
            StartCoroutine(Shoot());
            GameManager.Instance.RegisterEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Anim.GetBool("Is Dead"))
        {
            LookAtTarget();
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
        UpdateAnimBlend();
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() =>
        {
            return  m_Agent.remainingDistance < 0.02f;
        });

        while (!m_Damagable.IsDead)
        {
            m_Shotfx.Play();
            m_DamageShotFx.transform.rotation = Quaternion.LookRotation(transform.forward + Random.insideUnitSphere * 0.1f);

            if (Random.Range(0f, 1f) < m_ShootAccuracy)
            {
                m_DamageShotFx.Play();
                m_DamageShotFx.transform.rotation = Quaternion.LookRotation(m_Player.position - m_DamageShotFx.transform.position);
            }

            yield return new WaitForSeconds(m_Interval.GetValue);
        }
    }

    private void UpdateAnimBlend()
    {
        if (m_Anim == null || !m_Anim.enabled || !m_Agent.enabled) return;

        if (m_Agent.remainingDistance > 0.01f)
        {
            m_LocalMovement = Vector3.Lerp(m_LocalMovement, transform.InverseTransformDirection(m_Agent.velocity).normalized, 2f * Time.deltaTime);

            m_Agent.nextPosition = transform.position;
        }
        else
        {
            m_LocalMovement = Vector3.Lerp(m_LocalMovement, Vector3.zero, 2f * Time.deltaTime);
        }

        m_Anim.SetFloat("X Speed", m_LocalMovement.x);
        m_Anim.SetFloat("Z Speed", m_LocalMovement.z);
    }

    private void LookAtTarget()
    {
        if (m_Player == null)
        {
            Debug.LogError("No player exists in the world! Please add one.");
            return;
        }

        Vector3 direction = m_Player.position - transform.position;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void StopShooting()
    {
        StopAllCoroutines();
    }

    public void KillSelf()
    {
        GetCurrentActivePoint().EnemyKilled();
        GameManager.Instance.EnemyKilled();
        StopShooting();
    }

    public void TriggerDeadAnim()
    {
        m_Anim.SetTrigger("Dead");
        m_Anim.SetBool("Is Dead", true);
    }
    public void TriggerDamagedAnim()
    {
        m_Anim.SetTrigger("Damaged");
    }

    public ActionPoint GetCurrentActivePoint()
    {
        return m_ActionPoint;
    }
}

[System.Serializable]
public struct IntervalRange
{
    [SerializeField]
    private float m_Min;

    [SerializeField]
    private float m_Max;

    public IntervalRange(float min, float max)
    {
        m_Min = min;
        m_Max = max;
    }

    public float GetValue { get => Random.Range(m_Min, m_Max);}
}
