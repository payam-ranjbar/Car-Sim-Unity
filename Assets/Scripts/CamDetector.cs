
using Cinemachine;
using UnityEditor;

using UnityEngine;

namespace Script
{
    public class CamDetector : MonoBehaviour
    {
        public CinemachineVirtualCamera cam;

        private CinemachineVirtualCamera[] _allCams;

        private bool _active;

        private int _highest;
        private void Awake()
        {
            _allCams = FindObjectsOfType<CinemachineVirtualCamera>();
        }

        private void Active()
        {
            foreach (var vCam in _allCams)
            {
                if (vCam.Priority > _highest)
                {
                    _highest = vCam.Priority;
                }
            }

            cam.Priority = _highest + 1;
            _active = true;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_active) return;
            if (other.CompareTag("Player"))
            {
                Active();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _active = false;
            }
            
        }

        #if UNITY_EDITOR      
        [MenuItem("Tools/SetCamsFollowTarget")]
        public static void SetCamFollow()
        {
            var obj =  Selection.gameObjects[0].transform;
            var cams = FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (var cinemachineVirtualCamera in cams)
            {
                if (cinemachineVirtualCamera.Follow != null)
                {
                    cinemachineVirtualCamera.Follow = obj;
                }
            }
        }
        #endif
        
    }
}