using System.Collections.Generic;
using System.Text;
using HotUpdate.GameFrameWork.Module;
using HotUpdate.GameFrameWork.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
        if (GUILayout.Button("清空赋值信息"))
        {
            for (int i = 0; i < targetIns.ObjList.Count; i++)
            {
                targetIns.ObjList[i] = null;
            }
            targetIns.ObjCount = 1;
        }

        if (GUILayout.Button("将信息复制到剪切板"))
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
            debugStr += "private class WinView\n";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                if (!nameDic.ContainsKey(targetIns.ObjList[i].Name))
                {
                    nameDic.Add(targetIns.ObjList[i].Name, false);
                    string typeStr = GetNameType(targetIns.ObjList[i].Obj);
                    debugStr += string.Format("public readonly {0} {1};\r\n", typeStr, targetIns.ObjList[i].Name);
                }
                else
                {
                    Debug.LogError(string.Format("有重复的Name   [{0}]", targetIns.ObjList[i].Name));
                }
            }

            debugStr += "public WinView(UIWindow uiWindow)\n";
            debugStr += "{\r\n";
            for (int i = 0; i < targetIns.ObjCount; i++)
            {
                string typeStr = GetNameType(targetIns.ObjList[i].Obj);
                debugStr += $"{targetIns.ObjList[i].Name} = uiWindow.GetCommon<{typeStr}>(\"{targetIns.ObjList[i].Name}\");\n";
            }
            debugStr += "}\n";
            debugStr += "}";
            GUIUtility.systemCopyBuffer = debugStr;
        }

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


    private static string GetNameType(Object obj)
    {
        string typeStr = "GameObject";
        GameObject gameObject = obj as GameObject;
        if (gameObject == null)
        {
            return "";
        }
        if (gameObject.GetComponent<TextMeshProUGUI>())
        {
            typeStr = "TextMeshProUGUI";
        }
        else if (gameObject.GetComponent<Button>())
        {
            typeStr = "Button";
        }
        else if (gameObject.GetComponent<Image>())
        {
            typeStr = "Image";
        }

        return typeStr;
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