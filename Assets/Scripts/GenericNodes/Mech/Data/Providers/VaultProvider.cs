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
                if (files[i].Attributes.HasFlag(FileAttributes.Hidden) || files[i].Attributes.HasFlag(FileAttributes.System)) {
                    continue;
                }
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
                Vaults[i].GetSprites(packageLabel, sprites);
            }
        }

        public VaultPackageContentSprites.SpriteSettings GetSprite(string id) {
            for (int i = 0; i < Vaults.Count; ++i) {
                var sprite = Vaults[i].GetSprite(id);
                if (sprite != null) {
                    return sprite;
                }
            }
            return null;
        }
    }
}