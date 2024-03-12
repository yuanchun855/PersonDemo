using System.Collections.Generic;
using System.Text;
using HotUpdate.GameFrameWork.Module;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CanEditMultipleObjects()]
    [CustomEditor(typeof(UIWindow), true)]
    public class UIWindowEditor: UnityEditor.Editor
    {
         bool mIsFoldout = true;

    void OnEnable()
    {
    }

    void OnDisable()
    {
        //将比ObjCount大的元素，在BindList中删除
        UIWindow targetIns = (UIWindow)target;
        if (targetIns.ObjList.Count > targetIns.ObjCount)
        {
            targetIns.ObjList.RemoveRange(targetIns.ObjCount, targetIns.ObjList.Count - targetIns.ObjCount);
        }
    }
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        UIWindow targetIns = (UIWindow)target;
        targetIns.ObjCount = EditorGUILayout.IntField("ObjCount ", targetIns.ObjCount);
        if (targetIns.ObjCount < 0) targetIns.ObjCount = 0;
        if (targetIns.ObjList == null)
        {
            targetIns.ObjList = new List<UIWindow.ViewBindItem>();
        }

        for (int i = targetIns.ObjList.Count; i < targetIns.ObjCount; i++)
        {
            targetIns.ObjList.Add(new UIWindow.ViewBindItem());
        }
        mIsFoldout = EditorGUILayout.Foldout(mIsFoldout, "ObjList", true);//是否展开
        if (mIsFoldout)
        {
            for (int i = 0; i < targetIns.ObjCount; i++)//按ObjCount，显示文本
            {
                EditorGUILayout.BeginHorizontal();
                targetIns.ObjList[i].Name =
                    EditorGUILayout.TextField("    " + i, targetIns.ObjList[i].Name);
                if (targetIns.ObjList[i] != null && targetIns.ObjList[i].Name != null && targetIns.ObjList[i].Name.Contains(" "))
                {
                    Debug.LogError(string.Format("取名不要含有空格 [{0}]", targetIns.ObjList[i].Name));
                    targetIns.ObjList[i].Name = targetIns.ObjList[i].Name.Replace(" ", "");
                }
                Object lastObj = targetIns.ObjList[i].Obj;
                targetIns.ObjList[i].Obj =
                    EditorGUILayout.ObjectField("", targetIns.ObjList[i].Obj, typeof(Object), GUILayout.MaxWidth(100)) as Object;

                //绑定后自动赋名字
                if (targetIns.ObjList[i].Obj != null &&
                    (targetIns.ObjList[i].Name == "" || targetIns.ObjList[i].Name == string.Empty || targetIns.ObjList[i].Name == null || lastObj != targetIns.ObjList[i].Obj))
                {
                    targetIns.ObjList[i].Name = targetIns.ObjList[i].Obj.name;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("重排"))
        {
            for (int i = 0, length = targetIns.ObjCount; i < length; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    targetIns.ObjList.RemoveAt(i);
                    i--;
                    length--;
                }
                if (targetIns.ObjList[i].Obj != null) targetIns.ObjList[i].Name = targetIns.ObjList[i].Obj.name;
            }
            targetIns.ObjCount = targetIns.ObjList.Count;
        }

        if (GUILayout.Button("Debug所有ViewObjName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    Debug.LogError("请检查是否有空的Object");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            debugStr += "public class ViewObj";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                {
                    nameDic.Add(targetIns.ObjList[i].Name, false);
                    string typeStr = GetNameType(targetIns.ObjList[i].Name);
                    debugStr += string.Format("public {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                }
                else
                {
                    Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                }
            }
            debugStr += "public ViewObj(UIViewBase view)";
            debugStr += "{\r\n";
            List<string> btnFields = new List<string>();
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                //if (i == 0)
                //{
                //    debugStr += string.Format("    if({0}!=null)return;\r\n", targetIns.ObjList[i].Name);
                //}
                if (typeStr.Equals("TextButton")||typeStr.Equals("Button"))
                {
                    btnFields.Add(targetIns.ObjList[i].Name);
                }
                debugStr += string.Format("{0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            debugStr += "}\r\n";
            debugStr += "}\r\n";
            debugStr += " private ViewObj mViewObj;\r\n    public void OpenWindow()\r\n    {\r\n        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);\r\n        base.OpenWin();\r\n";
            for (int i = 0; i < btnFields.Count; i++)
            {
                debugStr += string.Format("    void BtnEvt_Click{0}()\r\n", btnFields[i]);
                debugStr += "    {\r\n    }\r\n";
            }
            debugStr += "}";
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Debug.Log(debugStr);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        }
        if (GUILayout.Button("Debug Window、所有ViewObjName"))
        {
            string debugStr = "";        
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    Debug.LogError("请检查是否有空的Object");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            debugStr += "using System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\nusing System.IO;\r\nusing UnityEngine.UI;\r\n";
            debugStr += "public class  " + targetIns.gameObject.name + ": WindowBase{\r\n";
            debugStr += "public class ViewObj";
            debugStr += "{\r\n";
            List<string> btnFields = new List<string>();
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                {
                    nameDic.Add(targetIns.ObjList[i].Name, false);
                    string typeStr = GetNameType(targetIns.ObjList[i].Name);
                    debugStr += string.Format("public {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                    if (typeStr.Equals("TextButton")||typeStr.Equals("Button"))
                    {
                        btnFields.Add(targetIns.ObjList[i].Name);
                    }
                }
                else
                {
                    Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                }
            }
            debugStr += "public ViewObj(UIViewBase view)";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                if (i == 0)
                {
                    debugStr += string.Format("    if({0}!=null)return;\r\n", targetIns.ObjList[i].Name);
                }
                debugStr += string.Format("{0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            debugStr += "}\r\n";
            debugStr += "}\r\n";
            debugStr += " private ViewObj mViewObj;\r\n    public void OpenWindow()\r\n    {\r\n        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);\r\n        base.OpenWin();\r\n    }\r\n";
            for (int i = 0; i < btnFields.Count; i++)
            {
                debugStr += string.Format("    void BtnEvt_Click{0}()\r\n", btnFields[i]);
                debugStr += "    {\r\n    }\r\n";
            }
            debugStr += "}";
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Debug.Log(debugStr);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        }
        if (GUILayout.Button("创建窗口脚本"))
        {
            if (UnityEditor.EditorUtility.DisplayDialog("提示", "创建窗口脚本？", "确定", "取消"))
            {
                string outputPath = "Assets/HotFix/GameLogic/HotFix/UI/Logic/Window/";
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputPath + "/" + targetIns.gameObject.name + ".cs", false, new UTF8Encoding(true)))
                {
                    string debugStr = "";
                    for (int i = 0; i < targetIns.ObjCount; i++)
                    {
                        if (targetIns.ObjList[i].Obj == null)
                        {
                            Debug.LogError("请检查是否有空的Object");
                        }
                    }

                    Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
                    //进行debug
                    debugStr += "using System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\nusing System.IO;\r\nusing UnityEngine.UI;\r\nusing TMPro;\r\n";
                    debugStr += "public class  " + targetIns.gameObject.name + ": WindowBase{\r\n";
                    debugStr += "    public class ViewObj\r\n";
                    debugStr += "    {\r\n";
                    List<string> btnFields = new List<string>();
                    for (int i = 0; i < targetIns.ObjCount; i++)
                    {
                        if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                        {
                            nameDic.Add(targetIns.ObjList[i].Name, false);
                            string typeStr = GetNameType(targetIns.ObjList[i].Name);
                            if (typeStr.Equals("TextButton")||typeStr.Equals("Button"))
                            {
                                btnFields.Add(targetIns.ObjList[i].Name);
                            }
                            debugStr += string.Format("        public {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                        }
                        else
                        {
                            Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                        }
                    }

                    debugStr += "        public ViewObj(UIViewBase view)\r\n";
                    debugStr += "        {\r\n";
                    for (int i = 0; i < targetIns.ObjCount; i++)
                    {
                        string typeStr = GetNameType(targetIns.ObjList[i].Name);
                        debugStr += string.Format("            {0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
                    }

                    debugStr += "        }\r\n";
                    debugStr += "    }\r\n";
                    debugStr += "    private ViewObj mViewObj;\r\n    public void OpenWindow()\r\n    {\r\n        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);\r\n        base.OpenWin();\r\n        Init();\r\n    }\r\n    void Init()\r\n    {\r\n";

                    for (int i = 0; i < btnFields.Count; i++)
                    {
                        debugStr += string.Format("        mViewObj.{0}.SetOnAduioClick(BtnEvt_Click{0});\r\n", btnFields[i]);
                    }
                    debugStr += "    }\r\n";
                    for (int i = 0; i < btnFields.Count; i++)
                    {
                        debugStr += string.Format("    void BtnEvt_Click{0}()\r\n", btnFields[i]);
                        debugStr += "    {\r\n    }\r\n";
                    }
                    debugStr += " }";
                    sw.Write(debugStr);
                }
                AssetDatabase.Refresh();
                var obj = AssetDatabase.LoadAssetAtPath(outputPath + targetIns.gameObject.name + ".cs", typeof(Object));
                if (obj)
                {
                    AssetDatabase.OpenAsset(obj);
                }
            }
            
        }
        if (GUILayout.Button("Debug所有SmallItemObjName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    Debug.LogError("请检查是否有空的Object");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            debugStr += "public class SmallObj : SmallViewObj\r\n";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                {
                    nameDic.Add(targetIns.ObjList[i].Name, false);
                    string typeStr = GetNameType(targetIns.ObjList[i].Name);
                    debugStr += string.Format("public {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                }
                else
                {
                    Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                }
            }
            debugStr += " public override void Init(UIViewBase view)\r\n";
            debugStr += "{\r\n";
            debugStr += "    base.Init(view);\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                debugStr += string.Format("{0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            debugStr += "}\r\n";
            debugStr += "}\r\n";
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Debug.Log(debugStr);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        }
        if (GUILayout.Button("Debug所有ObjName"))
        {
            string debugStr = "";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (targetIns.ObjList[i].Obj == null)
                {
                    Debug.LogError("请检查是否有空的Object");
                }
            }
            Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
            //进行debug
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                {
                    nameDic.Add(targetIns.ObjList[i].Name, false);
                    string typeStr = GetNameType(targetIns.ObjList[i].Name);
                    debugStr += string.Format("public {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                }
                else
                {
                    Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                }
            }
            debugStr += "\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Name);
                debugStr += string.Format("{0} = view.GetCommon<{1}>(\"{0}\");\r\n", targetIns.ObjList[i].Name, typeStr);
            }
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Debug.Log(debugStr);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        }
        if (GUILayout.Button("子物体Scale 归一化"))
        {
           Transform[] trans = targetIns.GetComponentsInChildren<Transform>();
           foreach (Transform child in trans)
           {
               child.localScale = Vector3.one;
           }
           Debug.Log("子物体Scale 归一化完成");
        }
        if (GUILayout.Button("清空赋值信息"))
        {
            for (int i = 0; i < targetIns.ObjList.Count; i++)
            {
                targetIns.ObjList[i] = null;
            }
            targetIns.ObjCount = 1;
        }
        // if (GUILayout.Button("将text替换成MText"))
        // {
        //     EditorHelper.ReplaceTextByMText(targetIns.gameObject);
        // }

  /*      if (GUILayout.Button("将text替换成TMP"))
        {
            EditorHelper.ReplaceTextByTMP(targetIns.gameObject);
        }*/

        //拖拽到面板，就进行添加
        var eventType = Event.current.type;
        if (eventType == UnityEngine.EventType.DragUpdated || eventType == UnityEngine.EventType.DragPerform)
        {
            // Show a copy icon on the drag
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (eventType == UnityEngine.EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                int addCount = 0;
                foreach (var o in DragAndDrop.objectReferences)
                {
                    bool isExsit = false;
                    foreach (var item in targetIns.ObjList)
                    {
                        if(item.Obj == o || item.Name == o.name)
                        {
                            isExsit = true;
                            break;
                        }
                    }
                    if (isExsit)
                    {
                        Debug.LogError("添加物体失败，已存在此物体或存在同名:" + o.name, o);
                    }
                    else
                    {
                        addCount++;
                        targetIns.ObjList.Add(new UIWindow.ViewBindItem(o.name, o));
                        Debug.Log("添加物体成功:" + o.name, o);
                    }
                }
                targetIns.ObjCount += addCount;
            }

            Event.current.Use();
        }
    }

    public static string GetNameType(string name)
    {
        string typeStr = "GameObject";
        if (name.Contains("Part_")) typeStr = "GameObject";
        else if (name.Contains("Texture")) typeStr = "RawImage";
        else if (name.Contains("Text")) typeStr = "TextMeshProUGUI";
        else if (name.Contains("Mat")) typeStr = "Material";
        else if (name.Contains("Mask")) typeStr = "Button";
        else if (name.Contains("TBtn") || name.Contains("TextButton")) typeStr = "TextButton";
        else if (name.Contains("Btn") || name.Contains("Button")) typeStr = "TextButton";
        else if (name.Contains("Root") || name.Contains("Panel")) typeStr = "Transform";
        else if (name.Contains("Icon") || name.Contains("Image")) typeStr = "Image";
        else if (name.Contains("Scroller")) typeStr = "UIScrollerLine";
        else if (name.Contains("ScrollView")) typeStr = "ScrollRect";

        return typeStr;
    }
    }
}