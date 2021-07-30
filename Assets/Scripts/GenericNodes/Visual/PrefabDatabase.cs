using System.Collections.Generic;
using GenericNodes.Mech.Fields;
using GenericNodes.Utility;
using GenericNodes.Visual.GenericFields;
using GenericNodes.Visual.Nodes;
using GenericNodes.Visual.PopupMenus;
using UnityEngine;

namespace GenericNodes.Visual {
    public class PrefabDatabase : SingletonMonoBehaviour<PrefabDatabase> {
        [Header("Node Fields")]
        [SerializeField] private StringGenericField stringFieldPrefab;
        [SerializeField] private IntGenericField intFieldPrefab;
        [SerializeField] private FloatGenericField floatFieldPrefab;
        [SerializeField] private BoolGenericField boolFieldPrefab;
        [SerializeField] private TextGenericField textFieldPrefab;
        [SerializeField] private EnumGenericField enumFieldPrefab;
        [SerializeField] private NodeIdGenericField nodeIdGenericFieldPrefab;
        [SerializeField] private CustomObjectGenericField customObjectDataFieldPrefab;
        [SerializeField] private CustomArrayGenericField customArrayFieldPrefab;
        [SerializeField] private SpriteAssetGenericField spriteAssetFieldPrefab;
        [Header("Popups")]
        [SerializeField] private PopupMenuItem prefabPopupMenuItem;
        [SerializeField] private PopupMenuCategory prefabPopupMenuCategory;
        [Header("Nodes")]
        [SerializeField] private NodeVisual prefabGenericNode;
        
        private readonly Dictionary<DataType, GameObject> fieldPrefabs = new Dictionary<DataType, GameObject>();

        public PopupMenuItem PrefabPopupMenuItem => prefabPopupMenuItem;
        public PopupMenuCategory PrefabPopupMenuCategory => prefabPopupMenuCategory;
        public NodeVisual PrefabGenericNode => prefabGenericNode;
        
        private void Awake() {
            fieldPrefabs.Add(DataType.String, stringFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Int, intFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Float, floatFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Bool, boolFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Text, textFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Enum, enumFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.NodeId, nodeIdGenericFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.CustomObject, customObjectDataFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.GenericArray, customArrayFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.SpriteAsset, spriteAssetFieldPrefab.gameObject);
        }

        public static GameObject GetFieldPrefab(DataType type) {
            return Instance.fieldPrefabs[type];
        }
    }
}