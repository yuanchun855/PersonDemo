using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class ResourceManager : BaseManager<ResourceManager>
{
    public enum AssetType
    {
        None = 0,
        Window = 1,
        Monster = 2,
        BattlePrefab = 3,
    }

    public string GetAssetAddressByAssetType(AssetType assetType,string assetName)
    {
        string address;
        switch (assetType)
        {
            case AssetType.Window:
                address = $"windows/{assetName}.prefab";
                break;
            case AssetType.None:
                address = $"{assetName}";
                break;
            case AssetType.BattlePrefab:
                address = $"BattlePrefabs/{assetName}.prefab";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(assetType), assetType, null);
        }
        return address;
    }
    public delegate void LoadResourceCallback(GameObject resource);
    public delegate void LoadAtlasCallback(Object[] resource);
    public void LoadPrefabByAssetType(AssetType assetType, string assetName,LoadResourceCallback callback)
    {
        string address = GetAssetAddressByAssetType(assetType, assetName);
        Addressables.LoadAssetAsync<GameObject>(address).Completed+= (op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                callback?.Invoke(op.Result);
            }
            else
            {
                Debug.LogError($"Failed to load resource at address: {address}");
            }
        });
    }
    public void LoadAtlas(string atlasName,LoadAtlasCallback callback)
    {
        string address = GetAssetAddressByAssetType(AssetType.None, atlasName);
        Addressables.LoadAssetAsync<Object[]>(address).Completed+= (op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                callback?.Invoke(op.Result);
            }
            else
            {
                Debug.LogError($"Failed to load resource at address: {address}");
            }
        });
    }
    public void Init()
    {
        
    }
    public void ReleaseResource(GameObject resource)
    {
        Addressables.ReleaseInstance(resource);
    }
    
}
