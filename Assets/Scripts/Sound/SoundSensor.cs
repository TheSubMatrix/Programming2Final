using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SoundSensor : MonoBehaviour, ISoundReactable
{
    [SerializeField] List<string> m_SoundsToListenFor;
    public UnityEvent<ISoundReactable.HeardSoundInfo> HeardSound = new UnityEvent<ISoundReactable.HeardSoundInfo>();
    public void OnHeardSound(ISoundReactable.HeardSoundInfo soundHeard)
    {
        if(m_SoundsToListenFor.Find(soundName => soundName == soundHeard.SoundInfo.Name) != null)
        {
            HeardSound.Invoke(soundHeard);
        }
    }
}