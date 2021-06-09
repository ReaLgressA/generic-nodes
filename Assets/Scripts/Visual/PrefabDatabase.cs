using System.Collections.Generic;
using Mech.Fields;
using UnityEngine;
using Visual.GenericFields;

namespace Visual {
    public class PrefabDatabase : SingletonMonoBehaviour<PrefabDatabase> {
        [SerializeField] private StringGenericField stringFieldPrefab;
        [SerializeField] private IntGenericField intFieldPrefab;
        [SerializeField] private FloatGenericField floatFieldPrefab;
        [SerializeField] private BoolGenericField boolFieldPrefab;
        [SerializeField] private TextGenericField textFieldPrefab;
        
        private Dictionary<DataType, GameObject> fieldPrefabs = new Dictionary<DataType, GameObject>();

        private void Awake() {
            fieldPrefabs.Add(DataType.String, stringFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Int, intFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Float, floatFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Bool, boolFieldPrefab.gameObject);
            fieldPrefabs.Add(DataType.Text, textFieldPrefab.gameObject);
        }

        public static GameObject GetFieldPrefab(DataType type) {
            return Instance.fieldPrefabs[type];
        }
    }
}