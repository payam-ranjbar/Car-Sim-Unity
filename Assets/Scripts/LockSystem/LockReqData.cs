using System;

namespace LockSystem
{
    [Serializable]
    public struct LockReqData
    {
        public bool isLock;
        public int time;
    }
}