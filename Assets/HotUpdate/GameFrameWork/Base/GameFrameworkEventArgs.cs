using System;
using HotUpdate.GameFrameWork.ReferencePool;

namespace HotUpdate.GameFrameWork.Base
{
    public abstract class GameFrameworkEventArgs: EventArgs,IReference
    {

        public GameFrameworkEventArgs()
        {
            
        }
        public abstract void Clear();
    }
}