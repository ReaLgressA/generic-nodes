using GenericNodes.Mech.Data;
using GenericNodes.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views.Project {
    public class ProjectUIView : MonoBehaviour {
        [SerializeField] private CreateNewProjectView createNewProjectView;
        [SerializeField] private ProjectHierarchyUIView projectHierarchyView;
        [SerializeField] private RectTransform rtrPanelRoot;
        [SerializeField] private Button buttonToggleHide;
        [SerializeField] private TextMeshProUGUI textToggleHide;

        private bool isHidden = false;
        
        public void OpenProject(GenericNodesProjectInfo projectInfo) {
            projectInfo.BuildProviders();
            projectHierarchyView.gameObject.SetActive(true);
            createNewProjectView.gameObject.SetActive(false);
            projectHierarchyView.SetProject(projectInfo);
        }

        public void CloseProject() {
            projectHierarchyView.gameObject.SetActive(false);
            createNewProjectView.gameObject.SetActive(true);
            projectHierarchyView.SetProject(null);
        }
        
        public void OpenGraphFile(string filePath) {
            Debug.Log($"Open graph: {filePath}");
        }
        
        private void Awake() {
            createNewProjectView.Setup(this);
            projectHierarchyView.Setup(this);
            
            projectHierarchyView.gameObject.SetActive(false);
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