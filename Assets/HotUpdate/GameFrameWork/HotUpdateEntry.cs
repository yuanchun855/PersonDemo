using System;
using UnityEngine;


    public class HotUpdateEntry
    {
        private void Start()
        {            
            Debug.Log("热更新入口222");
            GameLauncher.Instance.Launch();
        }
        public static void Run()
        {
            Debug.Log("热更新入口222");
            GameLauncher.Instance.Launch();
        }
    }

