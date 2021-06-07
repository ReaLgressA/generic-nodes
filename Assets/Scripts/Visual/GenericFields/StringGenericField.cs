using TMPro;
using UnityEngine;

namespace Visual.GenericFields {
    public class StringGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TMP_InputField inputFieldContent;
        
        
    }
}