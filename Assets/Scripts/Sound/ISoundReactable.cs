using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundReactable
{
    public struct HeardSoundInfo
    {
        public Vector3 SoundLocation {  get; private set; }
        public SoundManager.SoundInfo SoundInfo { get; private set; }
        public HeardSoundInfo(Vector3 soundLocation, SoundManager.SoundInfo soundInfo)
        {
            this.SoundLocation = soundLocation;
            this.SoundInfo = soundInfo;
        }
    }
    public void OnHeardSound(HeardSoundInfo soundInfo);
}
