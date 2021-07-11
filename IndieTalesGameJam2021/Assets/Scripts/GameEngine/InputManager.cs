using UnityEngine;

public class InputManager : MonoBehaviour {
    [Space, SerializeField] private InputReader inputReader;

    private void OnEnable() {
        inputReader.EnableGameplayInput();
    }
    
}