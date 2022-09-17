using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float m_HealAmount;

    [SerializeField]
    private Damagable m_PlayerDamagable;

    public void Damage(float damageAmount, RaycastHit hit = default)
    {
        if (m_PlayerDamagable != null)
        {
            m_PlayerDamagable.Heal(m_HealAmount);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("No player damagable has been referenced in the inspector. Please assign one.");
        }
    }
}
