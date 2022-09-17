using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	[SerializeField]
	private Rigidbody m_Head = null;

	[SerializeField]
	private GameState m_DeadState;

	private Transform m_CameraStartParent;
	private Quaternion m_CameraStartRotation;
	private Vector3 m_CameraStartPosition;
	private Vector3 m_HeadStartPosition;
	private Quaternion m_HeadStartRotation;
	private Camera m_Camera;
	private Damagable m_PlayerDamagable;

	private void Start()
	{
		m_Camera = FindObjectOfType<Camera>();
		m_PlayerDamagable = transform.parent.GetComponentInChildren<Damagable>();

		m_Head.isKinematic = true;
		m_Head.gameObject.SetActive(false);

		// Camera set up
		m_CameraStartRotation = m_Camera.transform.localRotation;
		m_CameraStartPosition = m_Camera.transform.localPosition;
		m_CameraStartParent = m_Camera.transform.parent;

		// Player head set up
		m_HeadStartPosition = m_Head.transform.localPosition;
		m_HeadStartRotation = m_Head.transform.localRotation;

		m_PlayerDamagable.onDead.AddListener(OnDeath);
		m_PlayerDamagable.onDead.AddListener(OnDelay);
	}

	private void OnDeath()
	{
		m_Camera.transform.parent = m_Head.transform;
		m_Head.gameObject.SetActive(true);
		m_Head.isKinematic = false;
		m_Head.AddForce(Vector3.ClampMagnitude(new Vector3(1f, 0.5f, .2f) * 0.5f, 10f), ForceMode.Force);
		m_Head.AddRelativeTorque(new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 35, ForceMode.Force);
	}

	private void OnDelay()
    {
		this.DelayedAction(delegate { GameStateManager.Instance.EnterGameState(m_DeadState); }, 1.5f);
	}
}
