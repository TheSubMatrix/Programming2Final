using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
public class SoundManager : MonoBehaviour
{
    [SerializeField] List<SoundInfo> m_sounds = new List<SoundInfo>();
    public List<SoundInfo> Sounds {  get { return m_sounds; } }
    public static SoundManager instance;
    public ObjectPool<GameObject> soundPool;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            soundPool = new ObjectPool<GameObject>(OnCreateSound, OnGetFromPool, OnReturnToPool, OnDestroySound);
        }
        else
        {
            Destroy(this);
        }
    }
#nullable enable
    public SoundInfo? FindSoundInfoByName(string name)
    {
        return m_sounds.Find(sound => sound.Name == name);
    }
#nullable disable
    public GameObject PlaySound(Vector3 location, SoundInfo soundInfo)
    {
        GameObject sound = soundPool.Get();
        sound.transform.position = location;
        AudioSource source = SetupSound(sound, soundInfo);

        source.Play();
        Debug.Log(source.isPlaying);
        return sound;
    }
    public GameObject PlaySound(Transform transform, SoundInfo soundInfo)
    {
        GameObject sound = soundPool.Get();
        sound.transform.parent = transform;
        sound.transform.localPosition = Vector3.zero;
        AudioSource source = SetupSound(sound, soundInfo);

        source.Play();
        //Debug.Log("Play");
        return sound;
    }
    AudioSource SetupSound(GameObject sound, SoundInfo soundInfo)
    {
        AudioSource source = sound.GetComponent<AudioSource>();
        SoundBroadcaster broadcaster = sound.GetComponent<SoundBroadcaster>();
        broadcaster.Initialize(soundInfo);
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
        if (!soundInfo.Loop && soundInfo.AudioClip != null)
        {
            StartCoroutine(DestroySound(sound, soundInfo.AudioClip.length));
        }
        return source;
    }
    IEnumerator DestroySound(GameObject soundToDestroy, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        if(soundToDestroy != null)
        {
            soundPool.Release(soundToDestroy);
        }
    }
    GameObject OnCreateSound()
    {
        return new GameObject("Sound", typeof(AudioSource), typeof(SphereCollider), typeof(SoundBroadcaster));
    }
    void OnGetFromPool(GameObject objectFromPool)
    {
        objectFromPool.SetActive(true);
    }
    void OnReturnToPool(GameObject objectFromPool)
    {
        objectFromPool.SetActive(false);
    }
    private void OnDestroySound(GameObject soundToDestroy)
    {
        Destroy(soundToDestroy.gameObject);
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
        int _priority = 128;
        public int Priority
        {
            get { return _priority; }
            set { _priority = Mathf.Clamp(value, 0, 256); }
        }
        [SerializeField, Range(0, 1)]
        float _volume = 1;
        public float Volume
        {
            get { return _volume; }
            set { _volume = Mathf.Clamp(value, 0, 1); }
        }
        [SerializeField, Range(-3, 3)]
        float _pitch = 1;
        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = Mathf.Clamp(value, -3, 3); }
        }
        [SerializeField, Range(-1, 1)]
        float _stereoPan = 0;
        public float StereoPan
        {
            get { return _stereoPan; }
            set { _stereoPan = Mathf.Clamp(value, -1, 1); }
        }
        [SerializeField, Range(0, 1)]
        float _spatialBlend = 0;
        public float SpatialBlend
        {
            get { return _spatialBlend; }
            set { _spatialBlend = Mathf.Clamp(value, 0, 1); }
        }
        [SerializeField, Range(0, 1.1f)]
        float _reverbZoneMix = 1;
        public float ReverbZoneMix
        {
            get { return _reverbZoneMix; }
            set { _reverbZoneMix = Mathf.Clamp(value, 0, 1.1f); }
        }
        [SerializeField]
        float _minDistance = 1;
        public float MinDistance
        {
            get { return _minDistance; }
            set { _minDistance = Mathf.Clamp(value, 0, MaxDistance); }
        }

        [SerializeField]
        float _maxDistance = 500;
        public float MaxDistance
        {
            get { return _maxDistance; }
            set { _maxDistance = Mathf.Clamp(value, MinDistance, float.MaxValue); }
        }
        [SerializeField]
        float _listenerTriggerDistance = 150;
        public float ListenerTriggerDistance
        {
            get { return _listenerTriggerDistance; }
            set { _listenerTriggerDistance = Mathf.Clamp(value, 0, float.MaxValue); }
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
            float maxDistance = 500,
            float listenerTriggerDistance = 150
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
            ListenerTriggerDistance = listenerTriggerDistance;
        }
    }
}