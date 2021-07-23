using System.IO;

namespace VaultKeeper.Utility {
    public static class FileManagementExtensions {

        public static void DeleteDirectoryContent(string directoryPath) {
            if (!Directory.Exists(directoryPath)) {
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            foreach(FileInfo fileInfo in dir.GetFiles())
            {
                fileInfo.Delete();
            }
            foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
            {
                DeleteDirectoryContent(directoryInfo.FullName);
                directoryInfo.Delete();
            }
        } 
    }
}