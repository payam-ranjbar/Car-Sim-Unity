using System;
using UnityEngine;

namespace ScriptingUtils
{
    public class CameraControlManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject observer;
        [SerializeField] private GameObject playerObserver;

        private int _mode;

        private void Start()
        {
            ActiveOne(player);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                ChangeMode();
            }
        }

        private void ActiveOne(GameObject selected)
        {
            player.SetActive(false);
            observer.SetActive(false);
            playerObserver.SetActive(false);

            selected.SetActive(true);
        }

        private void ActiveView()
        {
            switch (_mode)
            {
                case 0 : ActiveOne(player); break;
                case 1 : ActiveOne(observer); break;
                case 2 :
                    ActiveOne(playerObserver); break;
            }
        }

        private void ChangeMode()
        {
            _mode++;

            if (_mode > 2)
            {
                _mode = 0;
            }

            ActiveView();
        }
    }
}