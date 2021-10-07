using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Variables/Int")]
    public class IntVariable : VariableAbstract
    {

        public int Value;

        public void SetValue(int value)
        {
            Value = value;
            if (Event != null) Event.Raise();
        }

        public void SetValue(IntVariable value)
        {
            SetValue(value.Value);
        }

        public void ApplyChange(int amount)
        {
            Value += amount;

            if (eventOnApplyChange != null) eventOnApplyChange.Raise(amount);

            if (Event != null) Event.Raise();
        }

        public void ApplyChange(IntVariable amount)
        {
            ApplyChange(amount.Value);
        }
    }
}
