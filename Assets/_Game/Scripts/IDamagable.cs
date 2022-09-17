using UnityEngine;

public interface IDamagable
{
    void Damage(float damageAmount, RaycastHit hit = default(RaycastHit));
}