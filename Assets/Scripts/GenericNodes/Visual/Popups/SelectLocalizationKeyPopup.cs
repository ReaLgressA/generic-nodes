using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Popups {
    public class SelectLocalizationKeyPopup : MonoBehaviour {
        [SerializeField] private RectTransform rtrListRoot;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonApply;
        [SerializeField] private Button buttonNewKey;
        [SerializeField] private TMP_InputField textInputSearch;
        [SerializeField] private TextMeshProUGUI textSelectedKey;
    }
}