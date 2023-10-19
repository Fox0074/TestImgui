using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.Structures
{
    public class BaseTableManager<T> where T : BaseEntity
    {
        public List<T> TableItems = new List<T>();
        public float AutoUpdateTimeout;

        protected bool _isAutoUpdate;
        protected Thread _autoupdateThread;
        protected KernalInterface _driver;

        public BaseTableManager(KernalInterface driver)
        {
            _driver = driver;
        }

        public virtual void UpdateData() { }

        public void StartAutoUpdate()
        {
            if (!_isAutoUpdate)
            {
                _autoupdateThread = new Thread(new ThreadStart(UpdateThread));
                _autoupdateThread.Start();
                _isAutoUpdate = true;
            }
        }

        public void StopAutoUpdate()
        {
            if (_isAutoUpdate)
            {
                _autoupdateThread?.Abort();
                _isAutoUpdate = false;
            }
        }

        protected virtual string GetEntityTypeName(IntPtr entity)
        {
            return "NotOverride";
        }

        protected virtual void UpdateThread()
        {
            while (true)
            {
                if (AutoUpdateTimeout != 0)
                    Thread.Sleep((int)(AutoUpdateTimeout * 1000));

                lock (TableItems)
                {
                    UpdateData();
                }
            }
        }
    }
}
