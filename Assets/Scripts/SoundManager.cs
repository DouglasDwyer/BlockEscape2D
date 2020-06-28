using System;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static AudioSource soundEffectSource;
    public static AudioSource musicSource;

    public static void PlaySoundEffect(string effectName)
    {
        soundEffectSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Effects/" + effectName), 1f);
    }
}