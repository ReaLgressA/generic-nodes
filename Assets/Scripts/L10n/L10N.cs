namespace L10n {
    public static class L10N {
        public static LanguageConfig Config { get; private set; }
        
        
        public static void Initialize(LanguageConfig languageConfig) {
            Config = languageConfig;
        }
    }
}