using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CustomWeaponData", menuName ="Weapon Data")]
public class WeaponData : ScriptableObject
{
    public System.Action<int> OnWeaponShot = delegate { };
    public Sprite GetWeaponIcon { get { return m_WeaponIcon; } } 

    [SerializeField]
    private FireType m_FireType;

    [SerializeField]
    private AmmoType m_AmmoType;

    [SerializeField]
    private float m_Rate = 0.15f;

    [SerializeField]
    private int m_MaxAmmo;

    [SerializeField]
    private float m_DamageInflict;

    [SerializeField]
    private bool m_IsDefault;

    [SerializeField]
    private GameObject m_MuzzleFX;

    [SerializeField] 
    private AudioGetter m_GunShotSfx;

    [SerializeField]
    private AudioGetter m_DryFireSfx;

    [SerializeField]
    private float m_FxScale = 0.1f;

    [SerializeField]
    private Sprite m_WeaponIcon;

    private ParticleSystem m_CachedFX;
    private Camera m_Camera;
    private Weapon m_BaseWeapon;
    private int m_CurrentAmmo;
    private float m_NextFireTime;
    private CameraPathMovement m_PathMovement;

    public void InitialiseWeapon(Camera cam, Weapon weapon, CameraPathMovement pathMovement)
    {
        this.m_Camera = cam;
        this.m_BaseWeapon = weapon;
        m_NextFireTime = 0;
        m_CurrentAmmo = m_MaxAmmo;
        OnWeaponShot(m_CurrentAmmo);
        m_PathMovement = pathMovement;

        if (m_MuzzleFX != null)
        {
            GameObject fx = Instantiate(m_MuzzleFX);
            fx.transform.localScale = Vector3.one * m_FxScale;
            m_BaseWeapon.SetMuzzleFX(fx.transform);
            m_CachedFX = fx.GetComponent<ParticleSystem>();
        }
    }

    public bool AddAmmo(int amount, AmmoType ammoType)
    {
        if (ammoType == m_AmmoType && m_AmmoType != AmmoType.UNLIMITED)
        {
            m_CurrentAmmo = amount;
            return true;
        }

        return false;
    }

    public void Update()
    {
        if (m_FireType == FireType.SINGLE)
        {
            if (Input.GetMouseButtonDown(0) && m_CurrentAmmo > 0 && !m_PathMovement.CurrentActionPoint.inCover)
            {
                Shoot();
                m_CurrentAmmo--;
                OnWeaponShot(m_CurrentAmmo);
            }
            else if (Input.GetMouseButtonDown(0) && m_CurrentAmmo <= 0)
            {
                AudioPlayer.Instance.PlaySFX(m_DryFireSfx, m_BaseWeapon.transform);
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time > m_NextFireTime && m_CurrentAmmo > 0)
            {
                Shoot();
                m_CurrentAmmo--;
                OnWeaponShot(m_CurrentAmmo);
                m_NextFireTime = Time.time + m_Rate;
            }
            else if (m_CurrentAmmo < 0)
            {
            }
        }

        if (m_IsDefault && Input.GetMouseButtonDown(1))
        {
            m_CurrentAmmo = m_MaxAmmo;
            OnWeaponShot(m_CurrentAmmo);
        }

        if (!m_IsDefault && m_CurrentAmmo <= 0)
        {
            m_BaseWeapon.SwitchWeapon();
        }
    }

    public void Shoot()
    {
        AudioPlayer.Instance.PlaySFX(m_GunShotSfx, m_BaseWeapon.transform);

        GameManager.Instance.ShotFired();

        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

        if (m_CachedFX != null)
        {
            Vector3 muzzlePos = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.2f));
            m_CachedFX.transform.position = muzzlePos;
            m_CachedFX.transform.rotation = Quaternion.LookRotation(ray.direction);
            m_CachedFX.Play();
        }

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.collider != null)
            {
                Damagable damagable = hit.collider.GetComponent<Damagable>();

                if (damagable != null && hit.collider.GetComponent<Enemy>())
                {
                    damagable.Damage(m_DamageInflict, hit);
                    GameManager.Instance.EnemiesHit();
                }
                else
                {
                    IDamagable Idamagable = hit.collider.GetComponent<IDamagable>();

                    if (Idamagable != null)
                    {
                        Idamagable.Damage(0, hit);
                    }
                }
            }
            return;
        }
    }
}

public enum AmmoType
{
    UNLIMITED,
    MACHINE_GUN,
    SHOTGUN
}

public enum FireType
{
    SINGLE,
    RAPID
}