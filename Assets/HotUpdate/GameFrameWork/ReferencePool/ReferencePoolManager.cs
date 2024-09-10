using System;
using System.Collections.Generic;

namespace HotUpdate.GameFrameWork.ReferencePool
{
    public static partial class ReferencePoolManager
    {
        private static readonly Dictionary<Type, ReferenceHelper> _referenceHelpers =
            new Dictionary<Type, ReferenceHelper>();
        private static bool _enableStrictCheck;

        public static bool EnableStrictCheck
        {
            get => EnableStrictCheck;
            set => EnableStrictCheck = value;
        }

        public static int Count => _referenceHelpers.Count;

        public static ReferencePoolInfo[] GetAllReferencePoolInfos()
        {
            int index = 0;
            ReferencePoolInfo[] referencePoolInfos = null;
            lock (_referenceHelpers)
            {
                referencePoolInfos = new ReferencePoolInfo[_referenceHelpers.Count];
                foreach (KeyValuePair<Type,ReferenceHelper> helper in _referenceHelpers)
                {
                    referencePoolInfos[index++] = new ReferencePoolInfo(helper.Key, helper.Value.UnUsedReferenceCount,
                        helper.Value.UsingReferenceCount, helper.Value.AcquireReferenceCount,
                        helper.Value.ReleaseReferenceCount, helper.Value.AddReferenceCount,
                        helper.Value.RemoveReferenceCount);
                }
            }

            return referencePoolInfos;
        }

        public static void ClearAll()
        {
            lock (_referenceHelpers)
            {
                foreach (KeyValuePair<Type,ReferenceHelper> helper in _referenceHelpers)
                {
                    helper.Value.RemoveAll();
                }
                _referenceHelpers.Clear();
            }
        }

        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceHelper(typeof(T)).Acquire<T>();
        }

        public static IReference Acquire(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            return GetReferenceHelper(referenceType).Acquire();
        }

        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                return;
            }

            Type referenceType = reference.GetType();
            InternalCheckReferenceType(referenceType);
            GetReferenceHelper(referenceType).Release(reference);
        }

        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceHelper(typeof(T)).Add<T>(count);
        }

        public static void Add(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceHelper(referenceType).Add(count);
        }
        
        
        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReferenceHelper(typeof(T)).Remove(count);
        }

        public static void Remove(Type referenceType,int count)
        {   
            GetReferenceHelper(referenceType).Remove(count);
        }
        
        
        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceHelper(typeof(T)).RemoveAll();
        }

        public static void RemoveAll(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceHelper(referenceType).RemoveAll();
        }
        
        private static void InternalCheckReferenceType(Type referenceType)  
        {
            if (!_enableStrictCheck)
            {
                return;
            }
            if (referenceType == null)
            {
                
            }

            if (referenceType.IsAbstract || !referenceType.IsClass)
            {
                
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                
            }
        }

        private static ReferenceHelper GetReferenceHelper(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new Exception("");
            }

            ReferenceHelper referenceHelper = null;
            lock (_referenceHelpers)
            {
                if (!_referenceHelpers.TryGetValue(referenceType,out referenceHelper))
                {
                    referenceHelper = new ReferenceHelper(referenceType);
                    _referenceHelpers.Add(referenceType,referenceHelper);
                }
            }
            return referenceHelper;
        }
    }
}