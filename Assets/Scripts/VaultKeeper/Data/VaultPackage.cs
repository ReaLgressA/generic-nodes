using System;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using VaultKeeper.Data.PackageContent;
using VaultKeeper.Utility;

namespace VaultKeeper.Data {
    [Serializable]
    public class VaultPackage {
        [HideInInspector, SerializeField] 
        private string name;
        [HideInInspector, SerializeField] 
        private VaultPackageContentSprites contentSprites = new VaultPackageContentSprites();
        
        public string Name { get => name; set => name = value; }

        public VaultPackageContentSprites ContentSprites => contentSprites;

        public VaultPackage() { }

        public VaultPackage(string name) {
            this.name = name;
        }

        public void PrepareForSave() {
            contentSprites.PrepareForSave();
        }

        public void PrepareAfterLoading() {
            contentSprites.PrepareAfterLoading();
        }
        
        public void SaveContent(string contentRootPath) {
            string packageRootPath = Path.Combine(contentRootPath, name);
            Directory.CreateDirectory(packageRootPath);
            contentSprites.SaveContent(packageRootPath);
        }

        public void Export(ZipOutputStream stream, string directoryRoot) {
            string directoryPackage = $"{directoryRoot}{Name}/"; 
            stream.CreateDirectoryEntry(directoryPackage);
            contentSprites.Export(stream, directoryPackage);
        }
        
        public async Task PrepareAfterImport(ZipFile zipFile, string directoryRoot) {
            string directoryPackage = $"{directoryRoot}{Name}/";
            await contentSprites.PrepareAfterImport(zipFile, directoryPackage);
        }
    }
}