using System.Collections;
using System.Collections.Generic;
using System.IO;
using cfg;
using SimpleJSON;
using UnityEngine;

public class DataProvider : BaseManager<DataProvider>
{
    public cfg.Tables Tables;
    public void Init()
    {
        Tables = new Tables(LoadByteBuf);
    }
    private static JSONNode LoadByteBuf(string file)
    {
        return JSON.Parse(File.ReadAllText(Application.dataPath + "/HotUpdate/GameFrameWork/ConfigSheetJsonData/" + file + ".json", System.Text.Encoding.UTF8));
    }
}
