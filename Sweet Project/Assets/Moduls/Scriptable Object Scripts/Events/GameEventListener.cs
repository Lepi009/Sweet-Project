using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com.LepiStudios.ScriptableObjects
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("The weight of the listener, the lowest value will called first")]
        public int weight;

        [Tooltip("Event to register with.")]
        public GameEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        private void OnEnable()
        {
            if (Event == null) return;
            Event.RegisterListener(this);
        }

        public void SetUpRuntime(GameEvent e, UnityAction a)
        {
            Event = e;
            Response = new UnityEvent();
            Response.AddListener(a);
            OnEnable();
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}
