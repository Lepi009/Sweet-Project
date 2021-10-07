using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Variables/Boolean")]
    public class BooleanVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public bool Value;

        public GameEvent Event;

        public void SetValue(bool value)
        {
            Value = value;
            if (Event != null) Event.Raise();
        }

        public void SetValue(BooleanVariable value)
        {
            SetValue(value.Value);
        }


        void OnValidate()
        {
            if (Event != null) Event.Raise();
        }

    }
}
