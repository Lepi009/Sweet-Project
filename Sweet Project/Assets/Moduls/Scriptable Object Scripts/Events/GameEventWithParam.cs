using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Events/WithParameter")]
    public class GameEventWithParam : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<GameEventListenerWithParam> eventListeners =
            new List<GameEventListenerWithParam>();

        public void Raise(object objectRef)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(objectRef);
        }

        public void RegisterListener(GameEventListenerWithParam listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListenerWithParam listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}
