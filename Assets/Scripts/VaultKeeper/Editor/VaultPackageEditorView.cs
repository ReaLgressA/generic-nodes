using UnityEditor;
using UnityEngine;
using VaultKeeper.Data;

namespace VaultKeeper.Editor {
    public class VaultPackageEditorView {
        
        private SpriteListEditorView spriteListEditorView = new SpriteListEditorView();

        public void DrawOnGUI(VaultPackage package, Rect windowRect) {
            GUILayout.BeginVertical();
            //var propertyName = packageProperty.FindPropertyRelative("name");
            package.Name = EditorGUILayout.TextField("Name", package.Name);
            spriteListEditorView.DrawOnGUI(package.ContentSprites, windowRect);
            //EditorGUILayout.PropertyField(propertyName, new GUIContent("Name:"));
            GUILayout.EndVertical();
        }

        public void Reset() {
            spriteListEditorView.Reset();
        }
    }
}