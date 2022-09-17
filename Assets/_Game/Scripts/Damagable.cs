using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event for when the entity is dead
/// </summary>
[System.Serializable]
public class OnDeadEvent : UnityEvent { }

/// <summary>
/// Event for when the entity is healed
/// </summary>
[System.Serializable]
public class OnHealedEvent : UnityEvent { }

/// <summary>
/// Event for when the enity is damaged
/// </summary>
[System.Serializable]
public class OnDamagedEvent : UnityEvent { }

public class Damagable : MonoBehaviour, IDamagable
{
    public System.Action<float> OnPlayerDamaged = delegate { };
    public bool IsDead { get { return m_IsDead; } set { m_IsDead = value; } }
    public float CurrentHealth { get { return m_CurrentHealth; } }

    public bool IsInvulnerable { get { return m_Invulnerable; } set { m_Invulnerable = value; } }

    public float MaxHealth { get { return m_MaxHealth; } }

    [SerializeField]
    private bool m_IsPlayer = false;

    [SerializeField]
    private float m_MaxHealth = 100f;

    [SerializeField]
    private bool m_IsHealable = true;

    [SerializeField]
    private bool m_Invulnerable = false;

    [Header("Assign if player")]

    [SerializeField]
    private AudioGetter m_PlayerHitSfx;

    [SerializeField]
    private AudioGetter m_PlayerDrinkSfx;

    [SerializeField]
    private AudioGetter m_PlayerDeathSfx;

    private float m_CurrentHealth;
    private bool m_IsDead = false;
    private bool m_IsDamaged = false;

    public OnDamagedEvent onDamaged;
    public OnHealedEvent onHealed;
    public OnDeadEvent onDead;

    private void Start()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void Damage(float damageAmount, RaycastHit hit = default(RaycastHit))
    {
        if (m_IsDead || m_Invulnerable) return;

        m_CurrentHealth -= damageAmount;
        onDamaged.Invoke();

        if (m_IsPlayer)
        {
            OnPlayerDamaged(damageAmount);
            AudioPlayer.Instance.PlaySFX(m_PlayerHitSfx, transform);
        }

        if (m_CurrentHealth <= 0f)
        {
            m_CurrentHealth = 0f;
            m_IsDead = true;
            onDead.Invoke();

            if (m_IsPlayer)
            {
                AudioPlayer.Instance.PlaySFX(m_PlayerDeathSfx, transform);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (m_IsPlayer && other.tag == "Bullet")
        {
            var enemyWhoFired = other.GetComponentInParent<Enemy>();
            Damage(enemyWhoFired.DamageAmount);
            m_IsDamaged = true;
        }
    }

    /// <summary>
    /// Heals this health.
    /// </summary>
    /// <param name="healAmount"> The amount you want to heal.</param>
    public void Heal(float healAmount)
    {
        if (m_IsDead) return;

        if (m_IsHealable)
        {
            if (m_IsPlayer)
            {
                AudioPlayer.Instance.PlaySFX(m_PlayerDrinkSfx, transform);
            }

            m_CurrentHealth += healAmount;
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
            onHealed.Invoke();
        }
    }
}
