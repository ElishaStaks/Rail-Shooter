using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] 
    private float m_DamageRadius;

    [SerializeField] 
    private float m_DelayUntilDestroy;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_DelayUntilDestroy);
        DamageNearbyObjects();
    }

    private void DamageNearbyObjects()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, m_DamageRadius);

        foreach (var col in cols)
        {
            IDamagable[] hitables = col.GetComponents<IDamagable>();

            RaycastHit hit;

            if (Physics.Raycast(transform.position, col.transform.position - transform.position, out hit))

            if (hitables != null && hitables.Length > 0)
            {
                foreach (var hitable in hitables)
                {
                    hitable.Damage(100, hit);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_DamageRadius);
    }

}
