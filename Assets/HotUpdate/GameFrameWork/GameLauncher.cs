using HotUpdate.GameFrameWork.Module;
using HotUpdate.GameFrameWork.Windows;
using UnityEngine;


public class GameLauncher : BaseManager<GameLauncher>
{
    public void Launch()
    {
        Debug.Log("热更新成功！！！！！");
        DataProvider.Instance.Init();
        PlayerData data = new PlayerData(1);
        string json = MessagePack.MessagePackSerializer.SerializeToJson<PlayerData>(data);
        Debug.Log(json);

        LoadAsset();
    }

    public void LoadAsset()
    {
        ResourceManager.Instance.LoadPrefabByAssetType(ResourceManager.AssetType.None,"UIRoot", (obj) =>
        {
            GameObject obj1 = GameObject.Instantiate(obj);
            UIManager.Instance.Init();
            EventManager.Instance.Init();
            UIManager.Instance.LoadWindow<Window_Start>((win) =>
            {
                win.Open();
            });
        });
    }
}
