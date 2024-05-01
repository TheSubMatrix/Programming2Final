using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] Distractor m_prefab;
    [SerializeField] Transform m_handBone;
    [SerializeField] Vector3 m_offset;
    [SerializeField] float throwDistance = 5;
    Distractor newDistractor;

    public void StartThrow()
    {
        newDistractor = Instantiate(m_prefab, m_handBone, false);
        newDistractor.transform.localPosition = m_offset;
    }
    public void LetGoOfThrowable()
    {
        newDistractor.m_rigidbody.isKinematic = false;
        newDistractor.transform.parent = null;
        newDistractor.m_rigidbody.AddForce(transform.forward * throwDistance, ForceMode.Impulse);
    }
}
