﻿﻿using UnityEngine;
using UnityEngine.Events;

namespace GameEventSystem
{
    public class GameEventListener : MonoBehaviour
    {
        [Header("Events")]
        public GameEvent[] Events;
        
        [Header("Responses")]
        public UnityEvent Response;

        private void OnEnable()
        {
            foreach(GameEvent e in Events)
            {
                e.RegisterListener(this);
            }
        }

        public void OnEventsRaised()
        {
            Response.Invoke();
        }
    }
}