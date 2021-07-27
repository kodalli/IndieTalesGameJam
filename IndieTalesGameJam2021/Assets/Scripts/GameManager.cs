using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }
   [SerializeField] private int sceneIndex = 0;
   
   private void Awake() {
      if (Instance == null) {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      } else if (Instance != this) {
         Destroy(gameObject);
      }
   }

   private void Start() {
      sceneIndex = SceneManager.GetActiveScene().buildIndex;
   }

   public void GoToNextScene() {
      if (sceneIndex < SceneManager.sceneCountInBuildSettings-1) {
         sceneIndex++;
         SceneManager.LoadScene(sceneIndex);
      }
      else {
         GoToMenu();
      }
   }

   public void GoToMenu() {
      sceneIndex = 0; 
      SceneManager.LoadScene(sceneIndex);
   }

   public void GoToCurrentScene() {
      SceneManager.LoadScene(sceneIndex);
   }

}
