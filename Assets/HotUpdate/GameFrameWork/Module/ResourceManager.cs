using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : BaseManager<ResourceManager>
{
    public enum AssetType
    {
        Window = 0,
        Monster = 1,
    }

    public string GetAssetAddressByAssetType(AssetType assetType,string assetName)
    {
        string address;
        switch (assetType)
        {
            case AssetType.Window:
                address = $"windows/{assetName}.prefab";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(assetType), assetType, null);
        }
        return address;
    }
    public delegate void LoadResourceCallback(GameObject resource);
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
    public void Init()
    {
        
    }
    public void ReleaseResource(GameObject resource)
    {
        Addressables.ReleaseInstance(resource);
    }
    
}
