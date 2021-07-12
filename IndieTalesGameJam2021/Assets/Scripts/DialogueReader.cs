using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame.DialogueGraph {
    public class DialogueReader : MonoBehaviour {
        [SerializeField] private DialogueContainer dialogue;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private AudioClip sound;

        private readonly List<Button> buttonList = new List<Button>();
        private bool isTyping;

        private void Start() {
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        private void ProceedToNarrative(string guid) {
            string text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == guid).DialogueText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == guid);

            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
                Destroy(button.gameObject);

            buttonList.Clear();
            foreach (var choice in choices) {
                var button = Instantiate(choicePrefab, buttonContainer);
                button.GetComponentInChildren<Text>().text = choice.PortName;
                button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGUID));
                buttonList.Add(button);
            }

            StartCoroutine(PlayDialogue(text));
        }

        IEnumerator PlayDialogue(string text) {
            ToggleButton(true);

            var count = 0;
            while (count <= text.Length) {
                yield return new WaitForSeconds(0.04f);
                dialogueText.text = text.Substring(0, count);
                SoundManager.Instance.PlaySound(sound);
                count++;
            }

            ToggleButton(false);
        }

        private void ToggleButton(bool state) {
            if (buttonList.Count > 0)
                buttonList[0].enabled = state;
        }
    }
}