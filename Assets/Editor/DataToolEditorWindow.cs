using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor
{
    public class DataToolEditorWindow : EditorWindow
    {
        public static DataToolConfig Config;
        public string tempPath = "";
        [MenuItem("Tools/配置表工具")]
        private static void ShowWindow()
        {
            var window = GetWindow<DataToolEditorWindow>();
            Config = AssetDatabase.LoadAssetAtPath<DataToolConfig>("Assets/Editor/DataToolConfigData.asset");
            Debug.Log(Config.DllPath); 
            window.titleContent = new GUIContent("配置表工具");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            Config.DllPath = EditorGUILayout.TextField("LuBanDll路径:", Config.DllPath);
            if (GUILayout.Button("修改路径", GUILayout.Width(90)))
            {
                tempPath =  EditorUtility.OpenFilePanel("LuBanDllPath", tempPath, "dll");
            }
            if (!tempPath.Equals(""))
            {
                Config.DllPath = tempPath;
            }
            EditorGUILayout.EndHorizontal();
            tempPath = "";
            EditorGUILayout.BeginHorizontal();
            Config.InputPath = EditorGUILayout.TextField("游戏配置文件目录:", Config.InputPath);
            if (GUILayout.Button("修改路径", GUILayout.Width(90)))
            {
                tempPath =  EditorUtility.SaveFolderPanel("ConfigPath", tempPath, "游戏配置文件目录");
            }

            if (!tempPath.Equals(""))
            {
                Config.InputPath = tempPath;
            }
            EditorGUILayout.EndHorizontal();
            tempPath = "";
            EditorGUILayout.BeginHorizontal();
            Config.OutPutPath = EditorGUILayout.TextField("生成脚本路径:",Config.OutPutPath);
            if (GUILayout.Button("修改路径", GUILayout.Width(90)))
            {
                tempPath = EditorUtility.SaveFolderPanel("Save Path",  Config.OutPutPath, "选择生成脚本路径");
            }
            if (!tempPath.Equals(""))
            {
                Config.OutPutPath = tempPath;
            }
            EditorGUILayout.EndHorizontal();
            tempPath = "";
            EditorGUILayout.BeginHorizontal();
            Config.JsonPath = EditorGUILayout.TextField("生成json数据路径:",Config.JsonPath);
            if (GUILayout.Button("修改路径", GUILayout.Width(90)))
            {
                tempPath = EditorUtility.SaveFolderPanel("Save Path",  Config.JsonPath, "选择生成json数据路径");
            }
            if (!tempPath.Equals(""))
            {
                Config.JsonPath = tempPath;
            }
            EditorGUILayout.EndHorizontal();
            tempPath = "";
            if (GUILayout.Button("生成"))
            {
                // foreach (var file in Directory.GetFiles($"{Config.OutPutPath}"))    
                // {
                //     File.Delete(file);
                // }
                string confPath = $"{Application.dataPath}/MiniTemplate/luban.conf";
                string cmd =
                    $"dotnet {Config.DllPath} -t all -c cs-simple-json -d json --conf {confPath} -x outputCodeDir={Config.OutPutPath} -x outputDataDir={Config.JsonPath}";
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe"; // 使用Windows命令行
                process.StartInfo.Arguments = "/c" + cmd; // 使用/c参数来执行命令并关闭命令行窗口
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // 启动进程
                process.Start();

                // 读取输出
                string output = process.StandardOutput.ReadToEnd();

                // 等待进程退出
                process.WaitForExit();
                // 输出结果
                Debug.Log("Command Output: " + output);
                AssetDatabase.Refresh();
            }
        }
    }
}