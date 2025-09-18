using UnityEditor;
    using UnityEngine;
    using System.IO;

    public class SpriteExporter
    {
        [MenuItem("Assets/Export SubSprites")]
        private static void ExportSelectedSprites()
        {
            foreach (Object selectedObject in Selection.objects)
            {
                if (selectedObject is Sprite sprite)
                {
                    ExportSprite(sprite);
                }
                else if (selectedObject is Texture2D texture)
                {
                    // If a texture is selected, you might want to export all its sub-sprites
                    // This would require iterating through the sprites defined in the Sprite Editor for that texture.
                    // For simplicity, this example focuses on exporting a single selected Sprite.
                }
            }
            AssetDatabase.Refresh(); // Refresh the Asset Database to show the new files
        }

        private static void ExportSprite(Sprite sprite)
        {
            if (sprite == null) return;

            // Ensure the texture is readable
            Texture2D originalTexture = sprite.texture;
            if (!originalTexture.isReadable)
            {
                Debug.LogError($"Texture '{originalTexture.name}' is not readable. Please enable 'Read/Write Enabled' in its Import Settings.");
                return;
            }

            // Create a new texture for the sprite slice
            Texture2D newTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = originalTexture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            newTexture.SetPixels(pixels);
            newTexture.Apply();

            // Save the new texture as a PNG
            byte[] bytes = newTexture.EncodeToPNG();
            string path = AssetDatabase.GetAssetPath(originalTexture);
            string directory = Path.GetDirectoryName(path);
            string fileName = $"{directory}/{sprite.name}.png";

            File.WriteAllBytes(fileName, bytes);
            Debug.Log($"Exported sprite '{sprite.name}' to: {fileName}");

            Object.DestroyImmediate(newTexture); // Clean up the temporary texture
        }
    }