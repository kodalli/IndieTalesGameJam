using UnityEngine;

public class InputManager : MonoBehaviour {
    [Space, SerializeField] private InputReader inputReader;

    [Space, SerializeField] private PlayerInputData playerInputData;

    private void OnEnable() {
        inputReader.EnableGameplayInput();
        playerInputData.RegisterEvents();
    }

    private void OnDisable() {
        playerInputData.UnregisterEvents();
    }

    private void Start() {
        playerInputData.Reset();
    }
}