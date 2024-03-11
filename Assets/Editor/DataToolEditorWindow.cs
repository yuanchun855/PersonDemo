using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DataToolEditorWindow : EditorWindow
    {
        [MenuItem("Tools/配置表工具")]
        private static void ShowWindow()
        {
            var window = GetWindow<DataToolEditorWindow>();
            var dataConfig = AssetDatabase.LoadAssetAtPath<DataToolConfig>("Assets/Editor/DataToolConfigData.asset");
            Debug.Log(dataConfig.DllPath); 
            window.titleContent = new GUIContent("配置表工具");
            window.Show();
        }

        private void CreateGUI()
        {
            
        }
    }
}