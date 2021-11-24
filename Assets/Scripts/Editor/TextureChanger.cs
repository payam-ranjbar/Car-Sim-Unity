using UnityEditor;
using UnityEngine;

namespace CarEditor
{
    public class TextureChanger : EditorWindow
    {
        [SerializeField] private TextureImporter[] textures;
        [MenuItem("Tools/Change Texture Res")]
        private static void ShowWindow()
        {
            var window = GetWindow<TextureChanger>();
            window.titleContent = new GUIContent("Texture Change");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Change To Low"))
            {
                foreach (var texture in textures)
                {
                    Debug.Log("chnage");
                    texture.crunchedCompression = true;
                }
            }
        }
    }
}