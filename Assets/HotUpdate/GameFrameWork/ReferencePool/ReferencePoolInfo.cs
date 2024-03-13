using System;

namespace HotUpdate.GameFrameWork.ReferencePool
{
    public struct ReferencePoolInfo
    {
        private readonly Type _type;
        private readonly int _unUsedReferenceCount;
        private readonly int _usingReferenceCount;
        private readonly int _acquireReferenceCount;
        private readonly int _releaseReferenceCount;
        private readonly int _addReferenceCount;
        private readonly int _removeReferenceCount;

        public ReferencePoolInfo(Type type, int unUsedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            _type = type;
            _unUsedReferenceCount = unUsedReferenceCount;
            _usingReferenceCount = usingReferenceCount;
            _acquireReferenceCount = acquireReferenceCount;
            _releaseReferenceCount = releaseReferenceCount;
            _addReferenceCount = addReferenceCount;
            _removeReferenceCount = removeReferenceCount;
        }

        public Type Type => _type;
        public int UnUsedReferenceCount => _unUsedReferenceCount;
        public int UsingReferenceCount => _usingReferenceCount;
        public int AcquireReferenceCount => _acquireReferenceCount;
        public int ReleaseReferenceCount => _releaseReferenceCount;
        public int AddReferenceCount => _addReferenceCount;
        public int RemoveReferenceCount => _removeReferenceCount;
    }
}