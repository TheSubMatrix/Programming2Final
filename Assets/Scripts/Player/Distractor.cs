using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Distractor : MonoBehaviour
{
    public Rigidbody m_rigidbody {  get; private set; }
    [SerializeField] string soundToPlay;
    [SerializeField] float timeToWaitBeforeDestroy;
    [SerializeField] float timeToWaitBeforePlaying;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(timeToWaitBeforePlaying);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(transform, SoundManager.instance.FindSoundInfoByName(soundToPlay));
        }
        yield return new WaitForSeconds(timeToWaitBeforeDestroy);
        Destroy(gameObject);
    }
}
