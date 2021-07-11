using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }
   private int sceneIndex;
   [SerializeField] private SceneAsset[] scenes;
   
   private void Awake() {
      if (Instance == null) {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      } else if (Instance != this) {
         Destroy(gameObject);
      }
   }

   public void GoToNextScene() {
      if (sceneIndex < scenes.Length) {
         SceneManager.LoadScene(scenes[sceneIndex].name);
         sceneIndex++;
      }
   }
}
