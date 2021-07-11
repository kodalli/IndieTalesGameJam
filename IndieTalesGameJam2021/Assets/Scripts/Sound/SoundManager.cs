using System;
using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
    public static SoundManager Instance { get; private set; }

    private AudioSource source;

    private void Awake() {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip) {
        source.PlayOneShot(clip);
    }
}
