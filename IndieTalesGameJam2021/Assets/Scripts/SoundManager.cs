using System;
using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    private AudioSource source;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip) {
        source.PlayOneShot(clip);
    }

    public void PlayButtonClickSound() {
        source.PlayOneShot(buttonClickSound);
    }

    public void PlayDeathSound() {
        source.PlayOneShot(deathSound);
    }
}