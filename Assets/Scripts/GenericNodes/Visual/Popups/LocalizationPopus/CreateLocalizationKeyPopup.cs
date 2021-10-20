using System.Text.RegularExpressions;
using L10n;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Popups {
    public class CreateLocalizationKeyPopup : MonoBehaviour {
        private static readonly Regex invalidKeyRegex = new Regex("^(?![A-Za-z\\d_]+)$");
        private static readonly Regex validKeyRegex = new Regex("^[A-Za-z\\d_]+$");
        
        [SerializeField] private TMP_InputField textInputCategory;
        [SerializeField] private TMP_InputField textInputKey;
        [SerializeField] private TextMeshProUGUI textStatus;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonCreate;

        private SelectLocalizationKeyPopup SelectLocalizationKeyPopup { get; set; }
        
        private void Awake() {
            buttonClose.onClick.AddListener(Hide);
            buttonCreate.onClick.AddListener(CreateKey);
            textInputCategory.onValueChanged.AddListener(ProcessKeyUpdate);
            textInputKey.onValueChanged.AddListener(ProcessKeyUpdate);
        }
        
        private void OnDestroy() {
            buttonClose.onClick.RemoveAllListeners();
            buttonCreate.onClick.RemoveAllListeners();
        }

        public void Show(SelectLocalizationKeyPopup selectLocalizationKeyPopup) {
            SelectLocalizationKeyPopup = selectLocalizationKeyPopup;
            gameObject.SetActive(true);
            ProcessKeyUpdate(null);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        private void CreateKey() {
            L10N.RegisterKey(textInputCategory.text, textInputKey.text);
            SelectLocalizationKeyPopup.Refresh();
            Hide();
        }

        private void ProcessKeyUpdate(string _) {
            string category = invalidKeyRegex.Replace(textInputCategory.text.ToUpper(), string.Empty);
            textInputCategory.SetTextWithoutNotify(category);
            if (string.IsNullOrWhiteSpace(category)) {
                SetStatus("Category can't be empty!");
                return;
            }
            if (!validKeyRegex.IsMatch(category)) {
                SetStatus("Category has unsupported symbols!");
                return;
            }

            string key = invalidKeyRegex.Replace(textInputKey.text.ToUpper(), string.Empty);
            textInputKey.SetTextWithoutNotify(key);
            if (string.IsNullOrWhiteSpace(key)) {
                SetStatus("Key can't be empty!");
                return;
            }
            if (!validKeyRegex.IsMatch(key)) {
                SetStatus("Key has unsupported symbols!");
            }

            if (L10N.DoesKeyExist(category, key)) {
                SetStatus("Key already exists in this category!");
                return;
            }

            SetStatus(null);
        }

        private void SetStatus(string status) {
            textStatus.text = status;
            bool hasErrorStatus = !string.IsNullOrWhiteSpace(status);
            textStatus.color = hasErrorStatus ? Color.red : Color.white;
            buttonCreate.interactable = !hasErrorStatus;
        }
    }
}