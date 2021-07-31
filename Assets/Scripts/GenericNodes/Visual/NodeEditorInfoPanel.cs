using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Fields;
using GenericNodes.Utility;
using GenericNodes.Visual.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual
{
    public class NodeEditorInfoPanel : MonoBehaviour {
        [SerializeField] private WorkspaceArea workspaceArea;
        [SerializeField] private RectTransform rtrPanelRoot;
        [SerializeField] private Button buttonToggleHide;
        [SerializeField] private TextMeshProUGUI textToggleHide;
        [SerializeField] private TextMeshProUGUI textHeader;
        [SerializeField] private RectTransform rtrContentRoot;
        [SerializeField] private Button buttonSave;

        private readonly List<IGenericField> fields = new List<IGenericField>();
    
        private bool isHidden = false;

        public GraphData Data { get; private set; } = null;
        
        private void Awake() {
            buttonToggleHide.onClick.AddListener(ToggleHideWindow);
            buttonSave.onClick.AddListener(SaveGraph);
        }

        private void SaveGraph() {
            workspaceArea.ExportNodesToGraph();
            Data.SaveToFile();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.I) && !KeyboardInputManager.IsAnyGameObjectSelected) {
                ToggleHideWindow();
            }
        }

        private void OnDestroy() {
            buttonToggleHide.onClick.RemoveAllListeners();
            buttonSave.onClick.RemoveAllListeners();
        }

        private void ToggleHideWindow() {
            ToggleHideWindow(false);
        }

        private async void ToggleHideWindow(bool isForced) {
            if (Data == null && !isForced) {
                return;
            } 
            isHidden = !isHidden;
            textToggleHide.text = isHidden ? ">" : "<";
            await rtrPanelRoot.TweenAnchoredPos(isHidden ? new Vector2(-rtrPanelRoot.sizeDelta.x, 0f) : Vector2.zero,
                                                0.5f, Easings.EaseInOutQuad);
        }

        public void SetupData(GraphData data) {
            ClearFields();
            Data = data;
            workspaceArea.SetGraphData(data);
            if (data == null) {
                if (!isHidden) {
                    ToggleHideWindow(true);
                }
                return;
            }
            for (int i = 0; i < data.Info.Fields.Count; ++i) {
                GameObject goField = Instantiate(PrefabDatabase.GetFieldPrefab(data.Info.Fields[i].Type));
                var rtr = goField.GetComponent<RectTransform>();
                rtr.SetParent(rtrContentRoot);
                rtr.localScale = Vector3.one;
                rtr.SetAsLastSibling();
                IGenericField field = goField.GetComponent<IGenericField>();
                field.SetData(null, data.Info.Fields[i], null);
                fields.Add(field);
            }
            if (isHidden) {
                ToggleHideWindow(true);
            }

            workspaceArea.RebuildNodesFromGraphData();
        }

        private void ClearFields() {
            for (int i = 0; i < fields.Count; ++i) {
                fields[i].Destroy();
            }
            fields.Clear();
        }
    }
}
