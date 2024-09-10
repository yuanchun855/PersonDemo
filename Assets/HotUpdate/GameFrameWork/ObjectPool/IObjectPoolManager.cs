using System;
using System.Collections.Generic;

namespace HotUpdate.GameFrameWork.ObjectPool
{
    public interface IObjectPoolManager
    {
        int Count { get; }
        bool HasObjectPool<T>() where T : ObjectBase;
        bool HasObjectPool(Type objectType);
        bool HasObjectPool<T>(string name) where T : ObjectBase;
        bool HasObjectPool(string name, Type objectType);
        bool HasObjectPool(Predicate<ObjectPoolBase>condition);

        IObjectPool<T> GetObjectPool<T>() where T : ObjectBase;
        ObjectPoolBase GetObjectPool(Type objectType);
        
        IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase;
        ObjectPoolBase GetObjectPool(string name,Type objectType);
        ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase>condition);
        ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase>condition);
        void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results);

        ObjectPoolBase[] GetAllObjectPools();
        IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase;
        IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase;

        bool DestroyObjectPool<T>() where T : ObjectPoolBase;
        void Release();
        void ReleaseAllUnUsed();
    }
}