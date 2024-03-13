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
    }
}
