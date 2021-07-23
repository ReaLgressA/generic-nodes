using System;

namespace VaultKeeper.Data.PackageContent {
    
    [Serializable]
    public abstract class VaultPackageContent {
        
        public abstract VaultPackageContentType ContentType { get; }
    }
}