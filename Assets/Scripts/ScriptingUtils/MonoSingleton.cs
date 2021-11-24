﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ScriptingUtils
{


    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameObject().AddComponent<T>();

                return instance;
            }
        }

        public static bool IsInitialized;

        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            IsInitialized = true;
            Construct();
        }

        public static void Construct()
        {
            
        }

        public void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Other Instance of " + this.GetType().Name + "has been destroyed!");
            }

            instance = this as T;
        }
    }

}