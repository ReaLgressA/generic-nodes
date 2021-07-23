using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VaultKeeper.Data;

namespace VaultKeeper.Editor {
    public class VaultKeeperEditorWindow : EditorWindow {
        [MenuItem("Tools/Vault Keeper")]
        private static void ShowWindow() {
            var window = GetWindow<VaultKeeperEditorWindow>();
            window.titleContent = new GUIContent("Vault Keeper");
            window.Show();
        }
        
        private Vector2 scrollPackageList = new Vector2();
        private Vector2 scrollPackageInfoTab = new Vector2();
        private Vector2 scrollContentTab = new Vector2();
        
        private int selectedTab = 0;

        private ReorderableList listPackages;

        private VaultScriptableObjectWrapper vaultWrapper;
        private Vault Vault => vaultWrapper.vault;
        private SerializedObject soVault;
        private SerializedProperty packagesProperty;

        private VaultPackageEditorView vaultPackageEditorView = new VaultPackageEditorView();

        private int SelectedPackageIndex { get; set; } = -1;

        private void OnEnable() {
            vaultWrapper = (VaultScriptableObjectWrapper) CreateInstance(typeof(VaultScriptableObjectWrapper));
            SetVaultWrapper(vaultWrapper);
        }

        private void UpdatePackageSelection(ReorderableList list) {
            SelectPackage(list.index);
        }

        private void SelectPackage(int packageIndex) {
            SelectedPackageIndex = packageIndex;
            listPackages.ReleaseKeyboardFocus();
        }

        private void OnGUI() {
            soVault.Update();
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            DrawPackageList();
            DrawInfoTabs();
            GUILayout.EndHorizontal();

            DrawControlButtons();

            GUILayout.EndVertical();
            soVault.ApplyModifiedProperties();
        }

        private void DrawInfoTabs() {
            GUILayout.BeginVertical();
            selectedTab = GUILayout.Toolbar(selectedTab, tabList);
            switch (selectedTab) {
                case 0:
                    DrawPackageInfoTab();
                    break;
                case 1:
                    DrawVaultSettingsTab();
                    break;
            }
            GUILayout.EndVertical();
        }

        private void DrawVaultSettingsTab() {
            scrollContentTab = GUILayout.BeginScrollView(scrollContentTab);
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal(GUILayout.MaxHeight(32));
            GUILayout.Space(20);
            if (string.IsNullOrEmpty(Vault.FilePath)) {
                EditorGUILayout.HelpBox("Select file path to save!", MessageType.Warning);
            } else {
                EditorGUILayout.HelpBox($"Path: '{Vault.FilePath}'", MessageType.Info);
            }
            
            if (GUILayout.Button("Select Path", GUILayout.ExpandHeight(true))) {
                SaveVault();
            }
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void DrawPackageInfoTab() {
            scrollPackageInfoTab = GUILayout.BeginScrollView(scrollPackageInfoTab, GUILayout.Width(position.width * 3 / 4));
            if (SelectedPackageIndex >= 0 && SelectedPackageIndex < Vault.Packages.Count) {
                VaultPackage package = Vault.Packages[SelectedPackageIndex];
                vaultPackageEditorView.DrawOnGUI(package, position);
            }
            GUILayout.EndScrollView();
        }

        private void DrawPackageList() {
            scrollPackageList = GUILayout.BeginScrollView(scrollPackageList, 
                                                          GUILayout.Width(position.width / 4),
                                                          GUILayout.ExpandHeight(true));
            listPackages?.DoLayoutList();
            GUILayout.EndScrollView();
        }
        
        private void DrawControlButtons() {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Export", GUILayout.Height(32))) {
                ExportVault();
            }
            if (GUILayout.Button("Import", GUILayout.Height(32))) {
                ImportVault();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Load", GUILayout.Height(32))) {
                LoadVault();
            }
            if (GUILayout.Button("Save", GUILayout.Height(32))) {
                QuickSaveVault();
            }
            if (GUILayout.Button("Save As...", GUILayout.Height(32))) {
                QuickSaveVault();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Package", GUILayout.Height(32))) {
                Vault.AddPackage(new VaultPackage($"Package_{Vault.Packages.Count}"));
                SelectedPackageIndex = -1;
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Reset", GUILayout.Height(32))) {
                vaultWrapper = (VaultScriptableObjectWrapper) CreateInstance(typeof(VaultScriptableObjectWrapper));
                SetVaultWrapper(vaultWrapper);
                Reset();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        private void LoadVault() {
            string openDirectory = PlayerPrefs.GetString(LAST_PATH_DIR, string.Empty);
            string openFilePath = EditorUtility.OpenFilePanel("Open Vault File", openDirectory,
                                                              VaultConstants.VAULT_EXTENSION);
            if (string.IsNullOrEmpty(openFilePath)) {
                return;
            }
            vaultWrapper = Vault.LoadVault(openFilePath);
            PlayerPrefs.SetString(LAST_PATH_DIR, openFilePath);
            PlayerPrefs.Save();
            
            SetVaultWrapper(vaultWrapper);
        }

        private void QuickSaveVault() {
            if (!string.IsNullOrWhiteSpace(Vault.FilePath)) {
                SaveVault(Vault.FilePath);
            } else {
                SaveVault();
            }
        }

        private async void ImportVault() {
            string openFile = PlayerPrefs.GetString(LAST_EXPORT_PATH_DIR, string.Empty);
            string defaultName = string.IsNullOrWhiteSpace(openFile) ? "vault" :  Path.GetFileNameWithoutExtension(openFile);
            string importFilePath = EditorUtility.OpenFilePanel("Import Vault File", openFile, VaultConstants.ZIP_EXTENSION);
            Debug.Log($"Import file path: {importFilePath}");
            VaultScriptableObjectWrapper vault = await Vault.ImportVault(importFilePath);
            SetVaultWrapper(vault);
        }

        private void SaveVault() {
            string openDirectory = PlayerPrefs.GetString(LAST_PATH_DIR, string.Empty);
            string saveFilePath = EditorUtility.SaveFilePanel("Save Vault File", openDirectory,
                                                              "NewVault", VaultConstants.VAULT_EXTENSION);
            SaveVault(saveFilePath);
        }

        private void SaveVault(string saveFilePath) {
            if (string.IsNullOrEmpty(saveFilePath)) {
                return;
            }
            Debug.Log($"Save file path: {saveFilePath}");
            Vault.SaveVault(saveFilePath);
            PlayerPrefs.SetString(LAST_PATH_DIR, saveFilePath);
            PlayerPrefs.Save();
        }

        private void ExportVault() {
            
            string openFile = PlayerPrefs.GetString(LAST_EXPORT_PATH_DIR, string.Empty);
            string defaultName = string.IsNullOrWhiteSpace(openFile) ? "vault" :  Path.GetFileNameWithoutExtension(openFile);
            string exportFilePath = EditorUtility.SaveFilePanel("Save Vault File", openFile,
                                                              defaultName, VaultConstants.ZIP_EXTENSION);
            Debug.Log($"Export file path: {exportFilePath}");
            Vault.ExportVault(exportFilePath);
            PlayerPrefs.SetString(LAST_EXPORT_PATH_DIR, exportFilePath);
            PlayerPrefs.Save();
        }

        private void Reset() {
            SelectedPackageIndex = -1;
            vaultPackageEditorView.Reset();
        }
        
        private void SetVaultWrapper(VaultScriptableObjectWrapper vault) {
            vaultWrapper = vault;
            soVault = new SerializedObject(vault);
            SerializedProperty vaultProperty = soVault.FindProperty("vault");
            packagesProperty = vaultProperty.FindPropertyRelative("packages");
            listPackages = new ReorderableList(soVault, packagesProperty, true, true, true, true);
            listPackages.onSelectCallback += UpdatePackageSelection;
            Reset();
        }

        private const string LAST_PATH_DIR = "VAULT_KEEPER_LAST_PATH_DIR";
        private const string LAST_EXPORT_PATH_DIR = "VAULT_KEEPER_LAST_EXPORT_PATH_DIR";
        private const string TAB_PACKAGE_INFO = "Package Info";
        private const string TAB_VAULT_SETTINGS = "Vault Settings";
        private static readonly string[] tabList = { TAB_PACKAGE_INFO, TAB_VAULT_SETTINGS };
    }
}