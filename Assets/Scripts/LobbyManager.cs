using System;
using UnityEngine;

namespace CarDemo
{
    public class LobbyManager : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}