namespace GenericNodes.Visual.Views {
    public interface IFilePathEntry {
        bool IsDirectory { get; }
        string Path { get; }

        void SetSelected(bool isSelected);
    }
}