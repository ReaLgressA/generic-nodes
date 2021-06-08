using System.Collections.Generic;
using Mech.Fields;
using UnityEngine;
using Visual.GenericFields;

namespace Visual {
    public class PrefabDatabase : SingletonMonoBehaviour<PrefabDatabase> {
        [SerializeField] private StringGenericField stringFieldPrefab;

        private Dictionary<DataType, GameObject> fieldPrefabs = new Dictionary<DataType, GameObject>();

        private void Awake() {
            fieldPrefabs.Add(DataType.String, stringFieldPrefab.gameObject);
        }

        public static GameObject GetFieldPrefab(DataType type) {
            return Instance.fieldPrefabs[type];
        }
    }
}