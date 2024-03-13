using System;

namespace HotUpdate.GameFrameWork.ObjectPool
{
    public abstract class ObjectPoolBase
    {
        private readonly string _name;

        public ObjectPoolBase(string name)
        {
            _name = name ?? string.Empty;
        }

        public string Name => _name;
        public string FullName => ObjectType.FullName + _name;
        public abstract Type ObjectType
        {
            get;
        }

        public abstract int Count
        {
            get;
        }

        public abstract int CanReleaseCount
        {
            get;
        }

        public abstract bool AllowMultiSpawn
        {
            get;
        }

        public abstract float AutoReleaseInterval
        {
            get;
            set;
        }

        public abstract int Capacity
        {
            get;
            set;
        }
        public abstract int ExpireTime
        {
            get;
            set;
        }
        public abstract int Priority
        {
            get;
            set;
        }

        public abstract void Release();
        public abstract void Release(int releaseCount);
        public abstract void ReleaseAll();
        public abstract ObjectInfo[] GetAllObjectInfos();

        internal abstract void Update(float elapseSeconds, float realElapseSeconds);
        internal abstract void ShutDown();
    }
}