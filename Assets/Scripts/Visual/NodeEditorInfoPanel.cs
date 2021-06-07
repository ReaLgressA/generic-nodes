using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class NodeEditorInfoPanel : MonoBehaviour {
    [SerializeField] private RectTransform rtrPanelRoot;
    [SerializeField] private Button buttonToggleHide;
    [SerializeField] private TextMeshProUGUI textToggleHide;
    [SerializeField] private TextMeshProUGUI textHeader;
    [SerializeField] private RectTransform contentRoot;
    
    
    private bool isHidden = false;
    
    private void Awake() {
        buttonToggleHide.onClick.AddListener(ToggleHideWindow);
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

}
