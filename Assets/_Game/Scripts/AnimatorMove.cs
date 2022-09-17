using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMove : MonoBehaviour
{
    private Animator m_Anim;

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        if (m_Anim != null)
            transform.parent.position -= m_Anim.deltaPosition;
    }

    private void OnAnimatorMove()
    {
        if (m_Anim != null)
            transform.parent.position += m_Anim.deltaPosition;
    }
}
