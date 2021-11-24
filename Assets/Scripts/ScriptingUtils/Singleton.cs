﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ScriptingUtils
{

    public class Singleton<T> where T : class, new()
    {
        private static T instance;

        
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();

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
        
        public Singleton()
        {
            instance = this as T;
        }
    }

}