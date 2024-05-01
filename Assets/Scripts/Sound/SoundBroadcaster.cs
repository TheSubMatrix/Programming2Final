using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
public class SoundBroadcaster : MonoBehaviour
{
    AudioSource m_audioSource;
    SphereCollider m_sphereCollider;
    SoundManager.SoundInfo m_soundInfo;

    public void Initialize(SoundManager.SoundInfo soundInfo)
    {
        m_audioSource = GetComponent<AudioSource>();
        m_sphereCollider = GetComponent<SphereCollider>();
        m_sphereCollider.isTrigger = true;
        m_sphereCollider.radius = soundInfo.ListenerTriggerDistance;
        m_soundInfo = soundInfo;
        foreach (RaycastHit hits in Physics.SphereCastAll(transform.position, soundInfo.ListenerTriggerDistance, transform.forward))
        {
            ISoundReactable soundReactableObject = hits.transform.GetComponent<ISoundReactable>();
            if (soundReactableObject != null)
            {
                soundReactableObject.OnHeardSound(new ISoundReactable.HeardSoundInfo(m_audioSource.transform.position, soundInfo));
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        ISoundReactable soundReactable = other.GetComponent<ISoundReactable>();
        if (soundReactable != null)
        {
            soundReactable.OnHeardSound(new ISoundReactable.HeardSoundInfo(m_audioSource.transform.position, m_soundInfo));
        }
    }
}
