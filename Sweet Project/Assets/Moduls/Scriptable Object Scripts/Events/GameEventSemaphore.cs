using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Events/WithSemaphore")]
    public class GameEventSemaphore : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private List<GameEventListenerSemaphore> eventListeners =
            new List<GameEventListenerSemaphore>();

        public int sem = 0;

        public void Raise(bool inc)
        {
            if (inc) sem++;
            else sem--;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListenerSemaphore listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
            eventListeners.Sort((x, y) => y.weight.CompareTo(x.weight));
        }

        public void UnregisterListener(GameEventListenerSemaphore listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }


    }

}
