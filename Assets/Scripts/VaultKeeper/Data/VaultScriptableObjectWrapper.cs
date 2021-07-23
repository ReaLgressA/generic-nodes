using System;
using UnityEngine;

namespace VaultKeeper.Data {
    [Serializable]
    public class VaultScriptableObjectWrapper : ScriptableObject {
        public Vault vault = new Vault();
    }
}