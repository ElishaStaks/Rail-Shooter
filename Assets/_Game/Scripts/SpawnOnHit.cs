using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnOnHit : MonoBehaviour, IDamagable
{
    [SerializeField] GameObject prefabsToSpawn;
    [SerializeField] bool destroyOnHit;

    public void Damage(float damageAmount, RaycastHit hit = default)
    {
        if (prefabsToSpawn != null)
        {
            Instantiate(prefabsToSpawn, transform.position, Quaternion.identity);
        }

        if (destroyOnHit)
            Destroy(gameObject);
    }
}
