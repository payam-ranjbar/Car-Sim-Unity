﻿﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    [CreateAssetMenu(fileName = "Game Event", menuName = "Game Event", order = 0)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners;

        public event Action scriptingEvents;
        private void OnEnable()
        {
            listeners = new List<GameEventListener>();
        }

        public void Raise()
        {
            scriptingEvents?.Invoke();
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnEventsRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}