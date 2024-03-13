using System;
using System.Collections.Generic;

namespace HotUpdate.GameFrameWork.ObjectPool
{
    public interface IObjectPool<T> where T: ObjectBase
    {
        public delegate List<T> ReleaseObjectFilterCallback<T>(List<T> candidateObjects, int toReleaseCount, DateTime expireTime) where T : ObjectBase;
        string Name { get; }
        string FullName { get; }
        Type ObjectType { get; }
        int Count { get; }
        int CanReleaseCount { get; }
        bool AllowMultiplySpawn { get; }
        float AutoReleaseInterval { get; set; }
        int Capacity { get; set; }
        float ExpireTime { get; set; }
        int Priority { get; set; }

        void Register(T obj, bool isSpawned);
        bool CanSpawn();
        bool CanSpawn(string name);

        T GetSpawn();
        T GetSpawn(string name);

        void UnSpawn(T obj);
        void UnSpawn(object target);

        void SetLocked(T obj, bool locked);
        void SetLocked(object target, bool locked);

        void SetPriority(T obj, int priority);
        void SetPriority(object target, int priority);

        bool ReleaseObject(T obj);
        bool ReleaseObject(object target);

        void Release();

        void Release(int releaseCount);
        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="releaseObjectFilterCallback">释放对象筛选函数。</param>
        void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="releaseCount">尝试释放对象数量。</param>
        /// <param name="releaseObjectFilterCallback">释放对象筛选函数。</param>
        void Release(int releaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);
        void ReleaseAllUnUsed();

    }
}