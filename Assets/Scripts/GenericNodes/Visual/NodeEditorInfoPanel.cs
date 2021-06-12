using System.Collections.Generic;
using Mech.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Visual;

public class NodeEditorInfoPanel : MonoBehaviour {
    [SerializeField] private RectTransform rtrPanelRoot;
    [SerializeField] private Button buttonToggleHide;
    [SerializeField] private TextMeshProUGUI textToggleHide;
    [SerializeField] private TextMeshProUGUI textHeader;
    [SerializeField] private RectTransform rtrContentRoot;


    private readonly List<IGenericField> fields = new List<IGenericField>();
    
    private bool isHidden = false;
    
    private void Awake() {
        buttonToggleHide.onClick.AddListener(ToggleHideWindow);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I) && !KeyboardInputManager.IsAnyGameObjectSelected) {
            ToggleHideWindow();
        }
    }

    private void OnDestroy() {
        buttonToggleHide.onClick.RemoveAllListeners();
    }
    
    private async void ToggleHideWindow() {
        isHidden = !isHidden;
        textToggleHide.text = isHidden ? ">" : "<";
        await rtrPanelRoot.TweenAnchoredPos(isHidden ? new Vector2(-rtrPanelRoot.sizeDelta.x, 0f) : Vector2.zero,
                                            0.5f, Easings.EaseInOutQuad);
    }

    public void SetupData(string graphType, NodeData data) {
        textHeader.text = graphType;
        ClearFields();
        for (int i = 0; i < data.Fields.Count; ++i) {
            GameObject goField = Instantiate(PrefabDatabase.GetFieldPrefab(data.Fields[i].Type));
            var rtr = goField.GetComponent<RectTransform>();
            rtr.SetParent(rtrContentRoot);
            rtr.localScale = Vector3.one;
            rtr.SetAsLastSibling();
            IGenericField field = goField.GetComponent<IGenericField>();
            field.SetData(data.Fields[i]);
            fields.Add(field);
        }
    }

    private void ClearFields() {
        for (int i = 0; i < fields.Count; ++i) {
            fields[i].Destroy();
        }
        fields.Clear();
    }
}
