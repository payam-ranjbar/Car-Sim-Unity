using UnityEngine;

namespace LockSystem
{
    [CreateAssetMenu(menuName = "Create LockerProperties", fileName = "LockerProperties", order = 0)]
    public class LockerProperties : ScriptableObject
    {
        [SerializeField] private LockReqData data;
        [SerializeField] private string url;
        [SerializeField] private string token;

        
        private int _passedTime;

        public int PassedTime
        {
            get => _passedTime;
            set => _passedTime = value;
        }

        public bool IsLocked => data.isLock;

        public int SecondsToWait => data.time * 60;

        public string URL => url;

        public string Token => token;
        public string Path => "Core.data";

        public void SetData(LockReqData data)
        {
            this.data = data;
        }

        [ContextMenu("reset")]
        private void restTime()
        {
            _passedTime = 0;
        }
    }
}