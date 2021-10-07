using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com.LepiStudios.ScriptableObjects
{

    public class GameEventListenerWithParam : MonoBehaviour
    {
        [HideInInspector] public System.Object parameter;

        [Tooltip("Event to register with.")]
        public GameEventWithParam Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(object param)
        {
            this.parameter = param;
            Response.Invoke();
        }
    }
}