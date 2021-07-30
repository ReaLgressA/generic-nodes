using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VaultKeeper.Data;
using VaultKeeper.Data.PackageContent;

namespace GenericNodes.Mech.Data {
    public class VaultProvider {
        public List<Vault> Vaults { get; } = new List<Vault>();

        public async void Setup(GenericNodesProjectInfo projectInfo) {
            Vaults.Clear();
            string vaultsDirectoryPath = Path.Combine(projectInfo.AbsoluteRootPath, "Vaults");
            if (!Directory.Exists(vaultsDirectoryPath)) {
                Directory.CreateDirectory(vaultsDirectoryPath);
            }
            DirectoryInfo vaultsDirectory = new DirectoryInfo(vaultsDirectoryPath);
            FileInfo[] files = vaultsDirectory.GetFiles();
            
            for (int i = 0; i < files.Length; ++i) {
                try {
                    Vault vault = await Vault.ImportVault(files[i].FullName);
                    if (vault != null) {
                        Debug.Log($"Import vault {files[i].Name} with {vault.Packages} packages.");
                        Vaults.Add(vault);
                    } else {
                        Debug.LogError($"Failed to import vault by path: {files[i].FullName}");
                    }
                } catch (Exception ex) {
                    Debug.LogError($"Failed to parse vault file by path: {files[i].FullName}. Exception: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        public void GetSprites(string packageLabel, List<VaultPackageContentSprites.SpriteSettings> sprites) {
            sprites.Clear();
            for (int i = 0; i < Vaults.Count; ++i) {
                for (int j = 0; j < Vaults[i].Packages.Count; ++j) {
                    GetPackageSprites(Vaults[i].Packages[j], packageLabel, sprites);
                }
            }
        }

        private void GetPackageSprites(VaultPackage package, string packageLabel,
                                       List<VaultPackageContentSprites.SpriteSettings> sprites) {
            
            if (!string.IsNullOrWhiteSpace(packageLabel) 
                && !package.Label.Equals(packageLabel, StringComparison.Ordinal)) {
                return;
            }
            for (int i = 0; i < package.ContentSprites.Sprites.Count; ++i) {
                sprites.Add(package.ContentSprites.Sprites[i]);
            }
        }

        private VaultPackageContentSprites.SpriteSettings GetSprite(VaultPackage package, string id) {
            for (int i = 0; i < package.ContentSprites.Sprites.Count; ++i) {
                if (string.Equals(package.ContentSprites.Sprites[i].id, id, StringComparison.Ordinal)) {
                    return package.ContentSprites.Sprites[i];
                }
            }
            return null;
        }

        public VaultPackageContentSprites.SpriteSettings GetSprite(string id) {
            for (int i = 0; i < Vaults.Count; ++i) {
                for (int j = 0; j < Vaults[i].Packages.Count; ++j) {
                    var sprite = GetSprite(Vaults[i].Packages[j], id);
                    if (sprite != null) {
                        return sprite;
                    }
                }
            }
            return null;
        }
    }
}