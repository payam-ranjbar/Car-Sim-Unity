using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace LockSystem
{
    public class Locker : UnityEngine.MonoBehaviour
    {
        [SerializeField] private LockerProperties properties;
        [SerializeField] private int passUnit = 1;
        private int _passedSeconds;
        public void Start()
        {

            _passedSeconds = properties.PassedTime;
            // StartCoroutine(LockTimer());
            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }

        private IEnumerator LockTimer()
        {
            while (true)
            {
                if (_passedSeconds >= properties.SecondsToWait)
                {
                    Lock();
                    _passedSeconds = 0;
                    yield break;
                }
                yield return new WaitForSecondsRealtime(passUnit);
                _passedSeconds += passUnit;
                properties.PassedTime = _passedSeconds;
            }
        }

        private void Lock()
        {
            // Debug.Log("quit");
            // Application.Quit();
        }
    }
}
