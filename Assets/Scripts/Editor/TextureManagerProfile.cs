using System;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace GameGlobals
{
    
    [CreateAssetMenu(fileName = "Texture-Manager", menuName = "Texture Manager", order = -1)]
    public class TextureManagerProfile : ScriptableObject
    {
        [ContextMenuItem("Set", nameof(SetLowRes))]
        public TextureImporterSettingData lowResData;
        
        [ContextMenuItem("Set", nameof(SetMediumRes))]

        public TextureImporterSettingData mediumResData;
        [ContextMenuItem("Set", nameof(SetHighRes))]

        public TextureImporterSettingData highResData;
        
        
        public Texture[] textures;
        private TextureImporter _importer;


        [ContextMenu("Set Low")] 
        public void SetLowRes() => SetList(lowResData);
        [ContextMenu("Set Medium")] 
        public void SetMediumRes() => SetList(mediumResData);
        [ContextMenu("Set High")] 
        public void SetHighRes() => SetList(highResData);

        private void SetList(TextureImporterSettingData data) =>
            SetList((int) data.maxSize, data.compressionQuality, data.compression);
        private void SetList(int maxSize, int quality, TextureImporterCompression qualityMethod)
        {
            foreach (var texture in textures)
            {
                SetImporter(texture);
                SetTexture(maxSize, quality, qualityMethod);
            }
        }

        private void SetImporter(Texture texture)
        {
            _importer =  (TextureImporter) 
                AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture));
        }

        private void SetTexture(TextureImporterSettingData data) =>
            SetTexture((int) data.maxSize, data.compressionQuality, data.compression);
        private void SetTexture(int maxSize, int quality, TextureImporterCompression qualityMethod)
        {
            _importer.textureCompression = qualityMethod;
            _importer.compressionQuality = quality;
            _importer.maxTextureSize = maxSize;
            EditorUtility.SetDirty(_importer);
            _importer.SaveAndReimport();
        }
        
    }

    [Serializable]
    public enum TextureMaxSize
    {
        x32 = 32, x64 = 64, x128 = 128, x256 = 256, x512 = 512, x1024 = 1024, x2048 = 2048
    }
    [Serializable]
    public struct TextureImporterSettingData
    {
        public TextureMaxSize maxSize;
        public TextureImporterCompression compression;
        [Range(0, 100)] public int compressionQuality;
    }
}