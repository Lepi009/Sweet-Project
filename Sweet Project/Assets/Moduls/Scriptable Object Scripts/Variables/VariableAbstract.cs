using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.ScriptableObjects
{


    public abstract class VariableAbstract : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        [Tooltip("Event called afer applying or setting the variable")]
        public GameEvent Event;

        [Tooltip("Event called only on apply change with the parameter of the change")]
        public GameEventWithParam eventOnApplyChange;

        void OnValidate()
        {
            if (Event != null) Event.Raise();
        }
    }
}
