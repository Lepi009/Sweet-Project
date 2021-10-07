using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com.LepiStudios.ScriptableObjects
{


    [CreateAssetMenu(menuName = "Variables/Float")]
    public class FloatVariable : VariableAbstract
    {
        public float Value;

        public void SetValue(float value)
        {
            Value = value;
            if (Event != null) Event.Raise();
        }

        public void SetValue(FloatVariable value)
        {
            SetValue(value.Value);
        }

        public void ApplyChange(float amount)
        {
            Value += amount;

            if (eventOnApplyChange != null) eventOnApplyChange.Raise(amount);

            if (Event != null) Event.Raise();
        }

        public void ApplyChange(FloatVariable amount)
        {
            ApplyChange(amount.Value);
        }

    }

}
