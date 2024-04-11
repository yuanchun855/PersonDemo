namespace HotUpdate.GameFrameWork.Base.EventPool
{
    public abstract class BaseEventArgs: GameFrameworkEventArgs
    {
        public abstract int id
        {
            get;
        }
        public override void Clear()
        {
            
        }
    }
}