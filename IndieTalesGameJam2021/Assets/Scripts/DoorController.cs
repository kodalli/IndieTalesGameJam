using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
    [SerializeField] private AudioClip doorSound;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Debug.Log("Entered Door!");
            SoundManager.Instance.PlaySound(doorSound);
            GameManager.Instance.GoToNextScene();
        }
    }
}
