using System;
using System.Runtime.InteropServices;

namespace HotUpdate.GameFrameWork.ObjectPool
{
    [StructLayout(LayoutKind.Auto)]
    public struct ObjectInfo
    {
        private readonly string _name;
        private readonly bool _locked;
        private readonly bool _customReleaseFlag;
        private readonly int _spawnCount;
        private readonly int _priority;
        private readonly DateTime _lastUseTime;

        public ObjectInfo(string name, bool locked, bool customReleaseFlag, int spawnCount, int priority, DateTime lastUseTime)
        {
            _name = name;
            _locked = locked;
            _customReleaseFlag = customReleaseFlag;
            _spawnCount = spawnCount;
            _priority = priority;
            _lastUseTime = lastUseTime;
        }

        public string Name => _name;

        public bool Locked => _locked;

        public bool CustomReleaseFlag => _customReleaseFlag;

        public int SpawnCount => _spawnCount;
        public bool IsUsing => _spawnCount>0;

        public int Priority => _priority;

        public DateTime LastUseTime => _lastUseTime;
    }
}