using System;
using System.Threading;

namespace Biseth.Net.Couch.Threading
{
    public class BasicAsyncResult : IAsyncResult, IDisposable
    {
        private readonly object _asyncState;
        private readonly AsyncCallback _callback;
        private ManualResetEvent _waitHandle;

        internal BasicAsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _asyncState = state;
        }

        internal object InternalState { get; private set; }
        internal IAsyncResult InternalAsyncResult { get; set; }

        public object AsyncState
        {
            get { return _asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return CreateWaitHandle(); }
        }

        public Exception Exception { get; set; }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted { get; private set; }

        public void Dispose()
        {
            if (_waitHandle != null)
            {
                _waitHandle.Close();
            }
        }

        public void SetComplete()
        {
            IsCompleted = true;
            Thread.MemoryBarrier();
            if (_waitHandle != null)
            {
                _waitHandle.Set();
            }
            if (_callback != null)
            {
                _callback(this);
            }
        }

        private WaitHandle CreateWaitHandle()
        {
            if (_waitHandle != null)
            {
                return _waitHandle;
            }

            var newHandle = new ManualResetEvent(false);
            if (Interlocked.CompareExchange(ref _waitHandle, newHandle, null) != null)
            {
                newHandle.Close();
            }

            if (IsCompleted)
            {
                _waitHandle.Set();
            }

            return _waitHandle;
        }
    }
}