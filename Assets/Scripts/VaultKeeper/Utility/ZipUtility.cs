using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;

namespace VaultKeeper.Utility {
    public static class ZipUtility {

        public static async Task<string> LoadText(this ZipFile zipFile, string path) {
            ZipEntry vaultEntry = zipFile.GetEntry(path);
            using StreamReader reader = new StreamReader(zipFile.GetInputStream(vaultEntry));
            string json = await reader.ReadToEndAsync();
            return json;
        }
        
        public static async Task<Texture2D> LoadTexture2D(this ZipFile zipFile, string path,
                                                          TextureFormat textureFormat, bool isSRGB) {
            ZipEntry vaultEntry = zipFile.GetEntry(path);
            
            using Stream stream = zipFile.GetInputStream(vaultEntry);
            
            byte[] bytes = stream.ReadToEnd(); //ReadToEndAsync :(

            return TextureSerializer.LoadTexture(bytes, textureFormat, isSRGB);
        }
        
        public static void CreateDirectoryEntry(this ZipOutputStream stream, string directoryPath) {
            ZipEntry zipDir = new ZipEntry(directoryPath[directoryPath.Length - 1] == '/' 
                                               ? $"{directoryPath}" : $"{directoryPath}/");
            stream.PutNextEntry(zipDir);
            stream.CloseEntry();
        }

        public static void CreateFileEntry(this ZipOutputStream stream, string filePath, string content,
                                           CompressionMethod compressionMethod = CompressionMethod.Deflated) {
            ZipEntry z = new ZipEntry(filePath) { CompressionMethod = compressionMethod };
            stream.PutNextEntry(z);
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            stream.Write(bytes, 0, bytes.Length);
            stream.CloseEntry();
        }
        
        public static void CreateFileEntry(this ZipOutputStream stream, string filePath, byte[] bytes,
                                           CompressionMethod compressionMethod = CompressionMethod.Deflated) {
            ZipEntry z = new ZipEntry(filePath) { CompressionMethod = compressionMethod };
            stream.PutNextEntry(z);
            stream.Write(bytes, 0, bytes.Length);
            stream.CloseEntry();
        }
    }
}