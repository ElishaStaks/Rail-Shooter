using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static System.Action<WeaponData> OnWeaponChange = delegate { };
    public WeaponData CurrentWeapon { get => m_CurrentWeapon; }

    [SerializeField]
    private WeaponData m_WeaponData;

    private WeaponData m_CurrentWeapon;
    private Camera m_Camera;
    private Transform m_ChildFX;
    private CameraPathMovement m_PathMovement;

    private void Start()
    {
        m_Camera = GetComponentInParent<Camera>();
        m_PathMovement = GetComponentInParent<CameraPathMovement>();
        this.DelayedAction(delegate { SwitchWeapon(); }, 0.1f);
    }

    private void Update()
    {
        if (m_CurrentWeapon != null)
        {
            m_CurrentWeapon.Update();
        }
    }

    public virtual void SwitchWeapon(WeaponData weapon = null)
    {
        m_CurrentWeapon = weapon != null ? weapon : m_WeaponData;
        OnWeaponChange(m_CurrentWeapon);
        m_CurrentWeapon.InitialiseWeapon(m_Camera, this, m_PathMovement);
    }

    public void SetMuzzleFX(Transform fx)
    {
        if (m_ChildFX != null)
        {
            Destroy(m_ChildFX.gameObject);
        }

        fx.SetParent(transform);
        m_ChildFX = fx;
    }
}
