using System;
using System.Collections.Generic;

namespace HotUpdate.GameFrameWork.ObjectPool
{
    public sealed partial class ObjectPoolManager: BaseManager<ObjectPoolManager>,IObjectPoolManager
    {
        public int Count { get; }
        private const int DefaultCapacity  = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private const int DefaultPriority = 0;

        private Dictionary<string, ObjectPoolBase> _objectPoolDic;
        private List<ObjectPoolBase> _allCachedObjectPool;
        private Comparison<ObjectPoolBase> _comparisonObjectPool;

        public void Init()
        {
            _objectPoolDic = new Dictionary<string, ObjectPoolBase>();
            _allCachedObjectPool = new List<ObjectPoolBase>();
            _comparisonObjectPool = ObjectPoolComparer;
        }
        public bool HasObjectPool<T>() where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        public bool HasObjectPool(Type objectType)
        {
            throw new NotImplementedException();
        }

        public bool HasObjectPool<T>(string name) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        public bool HasObjectPool(string name, Type objectType)
        {
            throw new NotImplementedException();
        }

        public bool HasObjectPool(Predicate<ObjectPoolBase> condition)
        {
            throw new NotImplementedException();
        }

        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        public ObjectPoolBase GetObjectPool(string name, Type objectType)
        {
            throw new NotImplementedException();
        }

        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            throw new NotImplementedException();
        }

        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            throw new NotImplementedException();
        }

        public void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results)
        {
            throw new NotImplementedException();
        }

        public ObjectPoolBase[] GetAllObjectPools()
        {
            throw new NotImplementedException();
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase
        {
            throw new NotImplementedException();
        }

        public bool DestroyObjectPool<T>() where T : ObjectPoolBase
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        public void ReleaseAllUnUsed()
        {
            throw new NotImplementedException();
        }
        private static int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b)
        {
            return a.Priority.CompareTo(b.Priority);
        }
    }
}