using System;
using HotUpdate.GameFrameWork.ReferencePool;

namespace HotUpdate.GameFrameWork.ObjectPool
{
    public abstract class ObjectBase: IReference
    {
        private string _name;
        private object _target;
        private bool _locked;
        private int _priority;
        private DateTime _lastUseTime;

        public ObjectBase()
        {
            _name = null;
            _target = null;
            _locked = false;
            _priority = 0;
            _lastUseTime = default(DateTime);
        }

        public string Name => _name;

        public object Target => _target;

        public bool Locked
        {
            get => _locked;
            set => _locked = value;
        }

        public int Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public DateTime LastUseTime
        {
            get => _lastUseTime;
            set => _lastUseTime = value;
        }

        public virtual bool CustomCanReleaseFlag => true;


        protected void Initialize(object target)
        {
            Initialize(null,target,false,0);
        }
        
        protected void Initialize(string name,object target)
        {   
            Initialize(name,target,false,0);
        }

        protected void Initialize(string name, object target, bool locked)
        {
            Initialize(name,target,locked,0);
        }

        protected void Initialize(string name, object target, int priority)
        {
            Initialize(name,target,false,priority);
        }

        protected void Initialize(string name, object target, bool locked, int priority)
        {
            if (target == null)
            {
                //todo:1111
            }

            _name = name ?? string.Empty;
            _target = target;
            _locked = locked;
            _priority = priority;
            _lastUseTime = DateTime.UtcNow;
        }
        public virtual void Clear()
        {
            _name = null;
            _target = null;
            _locked = false;
            _priority = 0;
            _lastUseTime = default(DateTime);
        }

        protected internal virtual void OnSpawn(){}
        protected internal virtual void OnUnSpawn(){}
        protected internal abstract void Release(bool isShutDown);

    }
}