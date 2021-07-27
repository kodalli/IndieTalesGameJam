using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public void GoToNextScene() {
      GameManager.Instance.GoToNextScene();
   }

   public void ExitGame() {
      Application.Quit();
   }
}
