namespace L10n {
    public class LocalizedKeyDescription {
        public string Category { get; private set; }
        public string Key { get; private set; }

        public string LocalizationKey => $"@{Category}:{Key}";
        
        public LocalizedKeyDescription(string category, string key) {
            Category = category;
            Key = key;
        }
    }
}