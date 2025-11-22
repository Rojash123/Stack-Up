using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "Scriptable Objects/SoundSO")]
public class SoundSO : ScriptableObject
{
    public List<Sound> soundClass;
    [Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip clip;
    }

    public AudioClip GetSound(SoundType type)
    {
        Sound sound = soundClass.FirstOrDefault(x => type == x.type);
        if (sound != null)
        {
            return sound.clip;
        }
        return null;
    }
}
public enum SoundType
{
    platFormCut,
    platFormPerfectLanded,
    gameOver,
    endOfPlatform,
    UIClick
}


