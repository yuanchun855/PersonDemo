using System;
using UnityEngine;

public class HotUpdateEntry
{
    public static void Run()
    {
        Debug.Log("热更新入口222");
        GameLauncher.Instance.Launch();
    }
}