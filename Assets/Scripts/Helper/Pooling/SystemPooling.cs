using System.Collections.Generic;

namespace Scripts.Helper.Pooling
{
    public abstract class SystemPooling<T> : Singleton<SystemPooling<T>> where T : ObjectPooling
    {
        protected Dictionary<int, List<T>> _pool = new ();

        public abstract T Get(int itemID);

        public void Release(T oldObject)
        {
            oldObject.Release();
        }
    }
}