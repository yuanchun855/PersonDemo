using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdate.GameFrameWork.ReferencePool
{
    public partial class ReferencePoolManager
    {
        public sealed class ReferenceHelper
        {
            private readonly Queue<IReference> _references;
            private readonly Type _type;
            private int _usingReferenceCount;
            private int _acquireReferenceCount;
            private int _releaseReferenceCount;
            private int _addReferenceCount;
            private int _removeReferenceCount;

            public ReferenceHelper(Type type)
            {
                _references = new Queue<IReference>();
                _type = type;
                _usingReferenceCount = 0;
                _acquireReferenceCount = 0;
                _addReferenceCount = 0;
                _removeReferenceCount = 0;
                _releaseReferenceCount = 0;
            }

            public Type ReferenceType => _type;
            public int UnUsedReferenceCount => _references.Count;
            public int UsingReferenceCount => _usingReferenceCount;
            public int AcquireReferenceCount => _acquireReferenceCount;
            public int ReleaseReferenceCount => _releaseReferenceCount;
            public int AddReferenceCount => _addReferenceCount;
            public int RemoveReferenceCount => _removeReferenceCount;

            public T Acquire<T>() where T : class, IReference, new()
            {
                if (typeof(T) != _type)
                {
                    Debug.LogError("Type is InValid");
                }

                _usingReferenceCount++;
                _acquireReferenceCount++;
                lock (_references)
                {
                    if (_references.Count>0)
                    {
                        return (T)_references.Dequeue();
                    }
                }
                _addReferenceCount++;
                return new T();
            }

            public IReference Acquire()
            {
                _usingReferenceCount++;
                _acquireReferenceCount++;
                lock (_references)
                {
                    if (_references.Count>0)
                    {
                        return _references.Dequeue();
                    }
                }

                _addReferenceCount++;
                return (IReference)Activator.CreateInstance(_type);
            }

            public void Release(IReference reference)
            {
                reference.Clear();
                lock (_references)
                {
                    if (EnableStrictCheck && _references.Contains(reference))
                    {
                        throw new Exception("this reference has been exist");
                    }
                    _references.Enqueue(reference);
                }

                _releaseReferenceCount++;
                _usingReferenceCount--;
            }

            public void Add<T>(int count) where T : class, IReference, new()
            {
                if (typeof(T) != _type)
                {
                    throw new Exception("Type is InValid");
                }

                lock (_references)
                {
                    _addReferenceCount += count;
                    while (count-->0)
                    {
                        _references.Enqueue(new T());
                    }
                }
            }

            public void Add(int count)
            {
                lock (_references)
                {
                    _addReferenceCount += count;
                    while (count-->0)
                    {
                        _references.Enqueue((IReference)Activator.CreateInstance(_type));
                    }
                }
            }
            
            public void Remove(int count)
            {
                lock (_references)
                {
                    if (count > _references.Count)
                    {
                        count = _references.Count;
                    }

                    _removeReferenceCount += count;
                    while (count-- > 0)
                    {
                        _references.Dequeue();
                    }
                }
            }

            public void RemoveAll()
            {
                lock (_references)
                {
                    _removeReferenceCount += _references.Count;
                    _references.Clear();
                }
            }
            
        }
    }

}