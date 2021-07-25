using System.Collections;
using System.IO;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Popups;
using JsonParser;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views.Project {
    public class CreateNewProjectView : MonoBehaviour {
        private const string PROJECT_INFO_FILE_NAME = ".gnproject";
        
        [SerializeField] private Button buttonCreateNewProject;
        [SerializeField] private Button buttonOpenProject;

        private ProjectUIView ProjectView { get; set; }
        
        public void Setup(ProjectUIView projectUIView) {
            ProjectView = projectUIView;
        }
        
        private void Awake() {
            buttonOpenProject.onClick.AddListener(OpenProject);
            buttonCreateNewProject.onClick.AddListener(CreateNewProject);
        }
        
        private void OpenProject() {
            string lastPath = PlayerPrefs.GetString(LAST_OPEN_PATH, Application.dataPath);
            if (!Directory.Exists(lastPath) && !File.Exists(lastPath)) {
                lastPath = Application.dataPath;
            }
            PopupManager.GetPopup<SelectFilePathPopup>().Show(OpenProject, "Open",
                lastPath, string.Empty, SelectFilePathPopup.FileSelectionPolicy.OpenDirectory);
        }
        
        private void CreateNewProject() {
            string lastPath = PlayerPrefs.GetString(LAST_SAVE_PATH, Application.dataPath);
            if (!Directory.Exists(lastPath) && !File.Exists(lastPath)) {
                lastPath = Application.dataPath;
            }
            PopupManager.GetPopup<SelectFilePathPopup>().Show(CreateProject, "Create", 
                lastPath, "GraphProject", SelectFilePathPopup.FileSelectionPolicy.CreateDirectory);
        }

        private void OpenProject(string projectDirectoryPath) {
            if (string.IsNullOrWhiteSpace(projectDirectoryPath)) {
                return;
            }
            PlayerPrefs.SetString(LAST_OPEN_PATH, projectDirectoryPath);
            
            string projectInfoFilePath = Path.Combine(projectDirectoryPath, PROJECT_INFO_FILE_NAME);
            string projectInfoJson = File.ReadAllText(projectInfoFilePath);

            GenericNodesProjectInfo projectInfo = new GenericNodesProjectInfo();
            Hashtable ht = MiniJSON.JsonDecode(projectInfoJson) as Hashtable;
            if (ht != null) {
                projectInfo.FromJson(ht);
            }
            projectInfo.RootPath = Directory.GetParent(projectInfoFilePath)?.Parent?.FullName ?? string.Empty;
            ProjectView.OpenProject(projectInfo);
        }
        
        private void CreateProject(string projectDirectoryPath) {
            PlayerPrefs.SetString(LAST_SAVE_PATH, projectDirectoryPath);
            Directory.CreateDirectory(projectDirectoryPath);
            
            string projectInfoFilePath = Path.Combine(projectDirectoryPath, PROJECT_INFO_FILE_NAME);
            string rootPath = Directory.GetParent(projectDirectoryPath)?.FullName ?? string.Empty;
            string rootDirectoryName = new DirectoryInfo(projectDirectoryPath).Name;
            GenericNodesProjectInfo projectInfo = new GenericNodesProjectInfo {
                ProjectName = rootDirectoryName,
                RootDirectory = new GenericNodesProjectDirectory {
                    Name = rootDirectoryName
                },
                RootPath = rootPath
            };
            string infoJson = MiniJSON.JsonEncode(projectInfo, true);
            File.WriteAllText(projectInfoFilePath, infoJson);
            ProjectView.OpenProject(projectInfo);
        }
        
        private const string LAST_SAVE_PATH = "LAST_SAVE_PATH";
        private const string LAST_OPEN_PATH = "LAST_OPEN_PATH";
    }
}