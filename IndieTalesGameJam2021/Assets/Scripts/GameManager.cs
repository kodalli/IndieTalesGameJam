using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }
   [SerializeField] private int sceneIndex = 0;
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
      if (sceneIndex < scenes.Length-1) {
         sceneIndex++;
         SceneManager.LoadScene(scenes[sceneIndex].name);
      }
      else {
         GoToMenu();
      }
   }

   public void GoToMenu() {
      sceneIndex = 0; 
      SceneManager.LoadScene(scenes[0].name);
   }

   public void GoToCurrentScene() {
      SceneManager.LoadScene(scenes[sceneIndex].name);
   }
}
