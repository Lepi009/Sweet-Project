using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Events/Normal")]
    public class GameEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private List<GameEventListener> eventListeners =
            new List<GameEventListener>();

        public void Raise()
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
            eventListeners.Sort((x, y) => y.weight.CompareTo(x.weight));
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }

}
