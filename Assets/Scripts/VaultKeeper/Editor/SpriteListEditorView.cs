using System.Linq;
using UnityEditor;
using UnityEngine;
using VaultKeeper.Data.PackageContent;

namespace VaultKeeper.Editor {
    public class SpriteListEditorView {
        private int selectedEntryIndex = -1;

        private GUIStyle styleLabelCentered = null;
        
        private void InitializeStyles() {
            styleLabelCentered ??= new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleCenter
            };
        }
        
        public void DrawOnGUI(VaultPackageContentSprites content, Rect windowRect) {
            
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Label("Content List: Sprites", styleLabelCentered);
            InitializeStyles();
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
            if (Event.current.type == EventType.DragPerform) {
                DragAndDrop.AcceptDrag();
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++) {
                    Object obj = DragAndDrop.objectReferences[i];
                    Debug.Log(obj.GetType().Name);
                    if (obj is Sprite sprite)
                    {
                        content.Sprites.Add(new VaultPackageContentSprites.SpriteSettings(sprite));
                    } else if (obj is Texture2D texture) {
                        string texturePath = AssetDatabase.GetAssetPath(texture);
                        Object[] objects = AssetDatabase.LoadAllAssetsAtPath(texturePath);
                        Sprite[] sprites = objects.Where(q => q is Sprite).Cast<Sprite>().ToArray();
                        for (int j = 0; j < sprites.Length; ++j) {
                            content.Sprites.Add( new VaultPackageContentSprites.SpriteSettings(sprites[j]));   
                        }
                    }
                }
                Event.current.Use();
            }
            
            GUIContent[] spriteEntries = new GUIContent[content.Sprites.Count];
            for (int i = 0; i < content.Sprites.Count; ++i) {
                
                Texture2D preview = AssetPreview.GetAssetPreview(content.Sprites[i].sprite);
                GUIContent entryGUIContent = new GUIContent(preview);
                spriteEntries[i] = entryGUIContent;
            }
            int itemsPerRow = Mathf.FloorToInt(windowRect.width / 2 / 64f - 1);
            
            GUILayout.BeginHorizontal();            
            selectedEntryIndex = 
                GUILayout.SelectionGrid(selectedEntryIndex, spriteEntries, itemsPerRow, 
                                        GUILayout.Width(windowRect.width / 2 - 64f),
                                        GUILayout.Height(Mathf.Max(1, content.Sprites.Count / itemsPerRow) * 64f));
            GUILayout.Space(20);
            if (HasSelectedSprite(content)) {
                VaultPackageContentSprites.SpriteSettings selectedSprite = content.Sprites[selectedEntryIndex];
                
                Texture2D selectedPreview = AssetPreview.GetAssetPreview(selectedSprite.sprite);
                GUILayout.BeginVertical(GUILayout.Width(windowRect.width / 4));
                GUILayout.Box(selectedPreview, GUILayout.Width(windowRect.width / 4));
                selectedSprite.id = GUILayout.TextField(selectedSprite.id, GUILayout.Width(windowRect.width / 4));
                GUILayout.Label($"{selectedSprite.sprite.rect.width} x {selectedSprite.sprite.rect.height}", styleLabelCentered,
                                GUILayout.Width(windowRect.width / 4));
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (HasSelectedSprite(content) && GUILayout.Button("Remove")) {
                    content.Sprites.RemoveAt(selectedEntryIndex);
                }
                GUILayout.Space(25);
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
            } else {
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
        }

        public bool HasSelectedSprite(VaultPackageContentSprites content) {
            if (content?.Sprites == null || content.Sprites.Count == 0) {
                return false;
            }
            bool hasSelected = selectedEntryIndex >= 0 && selectedEntryIndex < content.Sprites.Count;
            if (hasSelected) {
                if (content.Sprites[selectedEntryIndex] != null) {
                    return true;
                }
                content.Sprites.RemoveAt(selectedEntryIndex);
                return false;
            }
            return false;
        }

        public void Reset() {
            selectedEntryIndex = -1;
        }
    }
}