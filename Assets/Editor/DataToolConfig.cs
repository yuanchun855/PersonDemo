using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataToolConfigData", menuName = "ScriptableObject/配置表工具数据", order = 0)]
public class DataToolConfig : ScriptableObject
{
    [SerializeField]
    public string DllPath;
    [SerializeField]
    public string InputPath;
    [SerializeField]
    public string OutPutPath;
    [SerializeField]
    public string JsonPath;
}
