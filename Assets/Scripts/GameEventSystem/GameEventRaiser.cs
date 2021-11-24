﻿﻿
using UnityEngine;

namespace GameEventSystem
{
    public class GameEventRaiser : MonoBehaviour
    {
        public void RaiseGameEvent(GameEvent Event)
        {
            if (Event != null)
            {
                Event.Raise();
            }
        }
    }
}