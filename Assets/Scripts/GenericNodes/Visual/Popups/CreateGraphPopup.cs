using System.Collections.Generic;
using System.IO;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Views.Project;
using JsonParser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Popups {
    public class CreateGraphPopup : MonoBehaviour {
        [SerializeField] private TMP_InputField textInputName;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Button buttonCreate;
        [SerializeField] private Button buttonClose;

        private string directoryPath;
        private GraphSchemeProvider schemeProvider;
        private ProjectUIView projectView;
        
        public void Show(string directoryPath, GraphSchemeProvider schemeProvider, ProjectUIView projectView) {
            this.directoryPath = directoryPath;
            this.schemeProvider = schemeProvider;
            this.projectView = projectView;
            dropdown.ClearOptions();
            List<string> schemesList = new List<string>(schemeProvider.Schemes.Count);
            for (int i = 0; i < schemeProvider.Schemes.Count; ++i) {
                schemesList.Add(schemeProvider.Schemes[i].Type);
            }
            dropdown.AddOptions(schemesList);
            dropdown.SetValueWithoutNotify(0);
            textInputName.text = $"New{schemesList[0]}";
            gameObject.SetActive(true);
        }

        private void Awake() {
            buttonClose.onClick.AddListener(Hide);
            buttonCreate.onClick.AddListener(CreateGraph);
        }

        private void OnDestroy() {
            buttonClose.onClick.RemoveAllListeners();
            buttonCreate.onClick.RemoveAllListeners();
        }

        private void Hide() {
            gameObject.SetActive(false);
        }

        private void CreateGraph() {
            GraphScheme scheme = schemeProvider.Schemes[dropdown.value];
            
            var filePath = Path.Combine(directoryPath, $"{textInputName.text}.json");
            GraphData graph = scheme.CreateGraph(filePath);
            graph.SaveToFile();
            projectView.OpenGraphFile(filePath);
            Hide();
        }
    }
}