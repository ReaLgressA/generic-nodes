using GenericNodes.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views.Project {
    public class ProjectUIView : MonoBehaviour {
        [SerializeField] private CreateNewProjectView createNewProjectView;
        [SerializeField] private RectTransform rtrPanelRoot;
        [SerializeField] private Button buttonToggleHide;
        [SerializeField] private TextMeshProUGUI textToggleHide;

        private bool isHidden = false;
        
        private void Awake() {
            createNewProjectView.gameObject.SetActive(true);
            buttonToggleHide.onClick.AddListener(ToggleHideWindow);
        }
        
        private void OnDestroy() {
            buttonToggleHide.onClick.RemoveAllListeners();
        }
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.P) && !KeyboardInputManager.IsAnyGameObjectSelected) {
                ToggleHideWindow();
            }
        }
        
        private async void ToggleHideWindow() {
            isHidden = !isHidden;
            textToggleHide.text = isHidden ? "<" : ">";
            await rtrPanelRoot.TweenAnchoredPos(isHidden ? new Vector2(rtrPanelRoot.sizeDelta.x, 0f) : Vector2.zero,
                                                0.5f, Easings.EaseInOutQuad);
        }
    }
}