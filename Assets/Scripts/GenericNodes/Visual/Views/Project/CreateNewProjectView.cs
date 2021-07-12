using GenericNodes.Visual.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views.Project {
    public class CreateNewProjectView : MonoBehaviour {
        [SerializeField] private Button buttonCreateNewProject;
        [SerializeField] private Button buttonOpenProject;
        
        private void Awake() {
            buttonOpenProject.onClick.AddListener(OpenProject);
            buttonCreateNewProject.onClick.AddListener(CreateNewProject);
        }
        
        private void OpenProject() {
            PopupManager.GetPopup<SelectFilePathPopup>().Show(OpenProject, "Open", 
                Application.dataPath, SelectFilePathPopup.FileSelectionPolicy.OpenDirectory);
        }
        
        private void CreateNewProject() {
            PopupManager.GetPopup<SelectFilePathPopup>().Show(CreateProject, "Create", 
                Application.dataPath, SelectFilePathPopup.FileSelectionPolicy.CreateDirectory);
        }

        private void OpenProject(string projectDirectoryPath) {
               
        }
        
        private void CreateProject(string projectDirectoryPath) {
            
        }
    }
}