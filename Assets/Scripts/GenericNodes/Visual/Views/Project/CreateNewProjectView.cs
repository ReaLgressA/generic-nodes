using System.IO;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views.Project {
    public class CreateNewProjectView : MonoBehaviour {
        private const string PROJECT_INFO_FILE_NAME = ".gn-project";
        
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
            Directory.CreateDirectory(projectDirectoryPath);
            string rootDirectoryName = Path.GetDirectoryName(projectDirectoryPath);
            
            string projectInfoFilePath = Path.Combine(projectDirectoryPath, PROJECT_INFO_FILE_NAME);
            GenericNodesProjectInfo projectInfo = new GenericNodesProjectInfo {
                ProjectName = Path.GetDirectoryName(projectDirectoryPath),
                RootDirectory = new GenericNodesProjectDirectory {
                    Name = rootDirectoryName
                },
                RootPath = Directory.GetParent(projectDirectoryPath)?.FullName ?? string.Empty
            };
            //File.OpenWrite(projectInfoFilePath);
        }
    }
}