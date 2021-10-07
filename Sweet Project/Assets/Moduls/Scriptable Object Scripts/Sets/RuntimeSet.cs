using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.ScriptableObjects
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        /// <summary>
        /// called after the set has changed
        /// </summary>
        public GameEvent EventOnChange;

        public List<T> Items = new List<T>();

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
            {
                Items.Add(thing);
                if (EventOnChange != null) EventOnChange.Raise();
            }
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
            {
                Items.Remove(thing);
                if (EventOnChange != null) EventOnChange.Raise();
            }
        }

        public void Clear()
        {
            Items.Clear();
            if (EventOnChange != null) EventOnChange.Raise();
        }
    }
}
