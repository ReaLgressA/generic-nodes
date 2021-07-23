using System.IO;
using UnityEngine;

namespace VaultKeeper {
    public static class TextureSerializer {
        
        public static Texture2D LoadTexture(byte[] bytes, TextureFormat textureFormat, bool isSRGB) {
            Texture2D texture2D = new Texture2D(1, 1, textureFormat, false, !isSRGB);
            texture2D.LoadImage(bytes, false);
            return texture2D;
        }

        public static void SaveSpriteAsTexture(Sprite sprite, string packagePath, string key) {
            int x = Mathf.FloorToInt(sprite.rect.x);
            int y = Mathf.FloorToInt(sprite.rect.y);
            int w = Mathf.Min(sprite.texture.width,Mathf.FloorToInt(sprite.rect.width));
            int h = Mathf.Min(sprite.texture.height,Mathf.FloorToInt(sprite.rect.height));
            
            Texture2D texture = new Texture2D(w, h, sprite.texture.format, false, false);
            Color[] pixels = sprite.texture.GetPixels(x, y, w, h);
            texture.SetPixels(0, 0, w, h, pixels);
            texture.Apply(false);
            SaveTexture(texture, packagePath, key);
        }
        
        public static void SaveTexture(Texture2D texture, string packagePath, string key) {
            byte[] bytes = texture.EncodeToPNG();
            string path = Path.Combine(packagePath, $"{key}.png");
            Directory.CreateDirectory(packagePath);
            File.WriteAllBytes(path, bytes);
        }

        public static byte[] GetSpriteBytes(Sprite sprite) {
            int x = Mathf.FloorToInt(sprite.rect.x);
            int y = Mathf.FloorToInt(sprite.rect.y);
            int w = Mathf.Min(sprite.texture.width,Mathf.FloorToInt(sprite.rect.width));
            int h = Mathf.Min(sprite.texture.height,Mathf.FloorToInt(sprite.rect.height));
            
            Texture2D texture = new Texture2D(w, h, TextureFormat.RGBA32, false, true);
            Color[] pixels = sprite.texture.GetPixels(x, y, w, h);
            texture.SetPixels(0, 0, w, h, pixels);
            texture.Apply(false);
            
            return texture.EncodeToPNG();
        }
    }
}