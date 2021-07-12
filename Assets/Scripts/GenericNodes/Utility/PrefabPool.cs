using System;
using System.Collections.Generic;
using UnityEngine;

namespace GenericNodes.Utility {
    [Serializable]
    public class PrefabPool<T> where T : MonoBehaviour {
        [SerializeField] private List<PoolEntry> poolEntries;

        private T prefab;
        private Transform trRoot;
        
        public PrefabPool(T prefab, Transform trRoot, int startCapacity) {
            this.prefab = prefab;
            this.trRoot = trRoot;
            poolEntries = new List<PoolEntry>(startCapacity);
            for (int i = 0; i < startCapacity; ++i) {
                SpawnExtraEntry();
            }
        }

        public T Request() {
            for (int i = 0; i < poolEntries.Count; ++i) {
                if (poolEntries[i].isRequested) {
                    continue;
                }
                poolEntries[i].isRequested = true;
                return poolEntries[i].entry;
            }
            PoolEntry extraEntry = SpawnExtraEntry();
            extraEntry.isRequested = true;
            return extraEntry.entry;
        }

        public void Release(T entry) {
            for (int i = 0; i < poolEntries.Count; ++i) {
                if (poolEntries[i].isRequested && poolEntries[i].entry == entry) {
                    MarkEntryAsReleased(poolEntries[i]);
                }
            }
        }

        public void ReleaseAll() {
            for (int i = 0; i < poolEntries.Count; ++i) {
                MarkEntryAsReleased(poolEntries[i]);
            }
        }

        private void MarkEntryAsReleased(PoolEntry poolEntry) {
            poolEntry.isRequested = false;
            poolEntry.entry.transform.SetParent(trRoot);
            poolEntry.entry.gameObject.SetActive(false);
        }
        
        private PoolEntry SpawnExtraEntry() {
            T entry = GameObject.Instantiate(prefab, trRoot);
            entry.transform.localScale = Vector3.one;
            entry.transform.localPosition = Vector3.zero;
            var poolEntry = new PoolEntry(entry, false);
            poolEntries.Add(poolEntry);
            return poolEntry;
        }

        private class PoolEntry {
            public readonly T entry;
            public bool isRequested = false;

            public PoolEntry(T entry, bool isRequested) {
                this.entry = entry;
                this.isRequested = isRequested;
            }
        }
    }
}