using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.myQuestSystem
{
    /// <summary>
    /// this class is the abstract parent class for all types of quest and defines the most common and needed methods
    /// </summary>
    public abstract class Quest
    {
        #region Attributes

        /// <summary>
        /// every quest should have a quest name which is saved in the QuestList enum
        /// </summary>
        public QuestList questName;

        /// <summary>
        /// every quest should have a describtion what is the aim and what to do
        /// </summary>
        private string myDescribtion;

        public delegate void DescribtionChanged(string newText);

        public event DescribtionChanged OnChangeDescribtion;

        #endregion

        #region Public Methods

        public string GetDescribtion()
        {
            return myDescribtion;
        }

        public void SetDescribtion(string newText)
        {
            myDescribtion = newText;
            OnChangeDescribtion.Invoke(newText);
        }

        /// <summary>
        /// the fire methods starts the quest and initalize the quest, e.g. spawning items, show something in a UI
        /// </summary>
        public abstract void Fire();

        /// <summary>
        /// the update method is called from outside and updates some values, e.g. count something up
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// the finish method is called to finish the quest, e.g. start next quest or brake some barricades
        /// </summary>
        public abstract void Finish();

        #endregion
    }

}
