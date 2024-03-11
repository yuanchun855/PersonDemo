using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataToolConfigData", menuName = "ScriptableObject/配置表工具数据", order = 0)]
public class DataToolConfig : ScriptableObject
{
    public string DllPath;
    public string InputPath;
    public string OutPutPath;
    public string JsonPath;
}
