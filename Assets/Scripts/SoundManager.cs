using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<String> sounds = new List<String>();
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject PlaySound(Vector3 location, SoundInfo soundInfo)
    {
        GameObject sound = new GameObject("Sound: " + soundInfo.Name, typeof(AudioSource), typeof(SphereCollider));
        sound.transform.position = location;
        AudioSource source = sound.GetComponent<AudioSource>();
        source.clip = soundInfo.AudioClip;
        source.outputAudioMixerGroup = soundInfo.AudioMixerGroup;
        source.mute = soundInfo.Mute;
        source.bypassEffects = soundInfo.BypassEffects;
        source.bypassListenerEffects = soundInfo.BypassListenerEffects;
        source.playOnAwake = soundInfo.PlayOnAwake;
        source.loop = soundInfo.Loop;
        source.priority = soundInfo.Priority;
        source.volume = soundInfo.Volume;
        source.pitch = soundInfo.Pitch;
        source.panStereo = soundInfo.StereoPan;
        source.spatialBlend = soundInfo.SpatialBlend;
        source.reverbZoneMix = soundInfo.ReverbZoneMix;
        source.minDistance = soundInfo.MinDistance;
        source.maxDistance = soundInfo.MaxDistance;
        if (!soundInfo.Loop)
        {
            StartCoroutine(DestroySound(sound, soundInfo.AudioClip.length));
        }
        foreach (RaycastHit hits in Physics.SphereCastAll(transform.position, source.maxDistance, transform.forward))
        {
            ISoundReactable soundReactableObject = hits.collider.gameObject.GetComponent<ISoundReactable>();
            if (soundReactableObject != null)
            {
                soundReactableObject.OnHeardSound(new ISoundReactable.HeardSoundInfo(sound.transform.position, soundInfo));
            }
        }
        source.Play();
        return sound;
    }
    public GameObject PlaySound(Transform transform, SoundInfo soundInfo)
    {
        GameObject sound = new GameObject("Sound: " + soundInfo.Name, typeof(AudioSource));
        sound.transform.parent = transform;
        AudioSource source = sound.GetComponent<AudioSource>();
        source.clip = soundInfo.AudioClip;
        source.outputAudioMixerGroup = soundInfo.AudioMixerGroup;
        source.mute = soundInfo.Mute;
        source.bypassEffects = soundInfo.BypassEffects;
        source.bypassListenerEffects = soundInfo.BypassListenerEffects;
        source.playOnAwake = soundInfo.PlayOnAwake;
        source.loop = soundInfo.Loop;
        source.priority = soundInfo.Priority;
        source.volume = soundInfo.Volume;
        source.pitch = soundInfo.Pitch;
        source.panStereo = soundInfo.StereoPan;
        source.spatialBlend = soundInfo.SpatialBlend;
        source.reverbZoneMix = soundInfo.ReverbZoneMix;
        source.minDistance = soundInfo.MinDistance;
        source.maxDistance = soundInfo.MaxDistance;
        
        if (!soundInfo.Loop)
        {
            StartCoroutine(DestroySound(sound, soundInfo.AudioClip.length));
        }
        source.Play();
        foreach(RaycastHit hits in Physics.SphereCastAll(transform.position, source.maxDistance, transform.forward))
        {
            ISoundReactable soundReactableObject = hits.collider.gameObject.GetComponent<ISoundReactable>();
            if (soundReactableObject != null)
            {
                soundReactableObject.OnHeardSound(new ISoundReactable.HeardSoundInfo(sound.transform.position, soundInfo));
            }
        }
        return sound;
    }
    IEnumerator DestroySound(GameObject soundToDestroy, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        if(soundToDestroy != null)
        {
            Destroy(soundToDestroy);
        }
    }
    [System.Serializable]
    public class SoundInfo
    {
        public string Name;
        public AudioClip AudioClip;
        public AudioMixerGroup AudioMixerGroup;
        public bool Mute;
        public bool BypassEffects;
        public bool BypassListenerEffects;
        public bool PlayOnAwake;
        public bool Loop;
        [SerializeField, Range(0, 256)]
        int _priority;
        public int Priority
        {
            get { return _priority; }
            set { _priority = Mathf.Clamp(value, 0, 256); }
        }
        [SerializeField, Range(0, 1)]
        float _volume;
        public float Volume
        {
            get { return _volume; }
            set { _volume = Mathf.Clamp(value, 0, 1); }
        }
        [SerializeField, Range(-3, 3)]
        float _pitch;
        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = Mathf.Clamp(value, -3, 3); }
        }
        [SerializeField, Range(-1, 1)]
        float _stereoPan;
        public float StereoPan
        {
            get { return _stereoPan; }
            set { _stereoPan = Mathf.Clamp(value, -1, 1); }
        }
        [SerializeField, Range(0, 1)]
        float _spatialBlend;
        public float SpatialBlend
        {
            get { return _spatialBlend; }
            set { _spatialBlend = Mathf.Clamp(value, 0, 1); }
        }
        [SerializeField, Range(0, 1.1f)]
        float _reverbZoneMix;
        public float ReverbZoneMix
        {
            get { return _reverbZoneMix; }
            set { _reverbZoneMix = Mathf.Clamp(value, 0, 1.1f); }
        }

        [SerializeField]
        float _maxDistance = 500;
        public float MaxDistance
        {
            get { return _maxDistance; }
            set { _maxDistance = Mathf.Clamp(value, MinDistance, float.MaxValue); }
        }
        [SerializeField]
        float _minDistance = 1;
        public float MinDistance
        {
            get { return _minDistance; }
            set { _minDistance = Mathf.Clamp(value, 0, MaxDistance); }
        }
        public SoundInfo
            (
            string name,
            AudioClip audioClip,
            AudioMixerGroup audioMixerGroup = null,
            bool mute = false,
            bool bypassEffects = false,
            bool bypassListenerEffects = false,
            bool playOnAwake = true,
            bool loop = false,
            int priority = 128,
            float volume = 1,
            float pitch = 1,
            float stereoPan = 0,
            float spatialBlend = 0,
            float reverbZoneMix = 1,
            float minDistance = 1,
            float maxDistance = 500
            )
        {
            Name = name;
            AudioClip = audioClip;
            AudioMixerGroup = audioMixerGroup;
            Mute = mute;
            BypassEffects = bypassEffects;
            BypassListenerEffects = bypassListenerEffects;
            PlayOnAwake = playOnAwake;
            Loop = loop;
            Priority = priority;
            Volume = volume;
            Pitch = pitch;
            StereoPan = stereoPan;
            SpatialBlend = spatialBlend;
            ReverbZoneMix = reverbZoneMix;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
        }
    }
}
