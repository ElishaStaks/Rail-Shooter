using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour, IDamagable
{
    [SerializeField]
    private GameObject m_HitEffectPrefab;

    private ParticleSystem m_HitEffectCache;

    private void Start()
    {
        if (m_HitEffectPrefab != null)
        {
            GameObject effect = Instantiate(m_HitEffectPrefab, transform);
            m_HitEffectCache = effect.GetComponent<ParticleSystem>(); 
        }
    }

    public void Damage(float damageAmount, RaycastHit hit = default(RaycastHit))
    {
        if (m_HitEffectCache != null)
        {
            m_HitEffectCache.transform.position = hit.point;
            m_HitEffectCache.transform.rotation = Quaternion.LookRotation(hit.normal);
            m_HitEffectCache.Play();
        }
    }
}
