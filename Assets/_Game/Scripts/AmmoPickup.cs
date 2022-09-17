using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField]
    private AmmoType m_AmmoType;

    [SerializeField]
    private int m_AmmoAmount;

    private Weapon m_Weapon;

    private void Start()
    {
        m_Weapon = FindObjectOfType<Weapon>();
    }

    public void Damage(float damageAmount, RaycastHit hit = default(RaycastHit))
    {
        var pickedup = m_Weapon.CurrentWeapon.AddAmmo(m_AmmoAmount, m_AmmoType);
        if (pickedup)
            Destroy(gameObject);
    }
}
