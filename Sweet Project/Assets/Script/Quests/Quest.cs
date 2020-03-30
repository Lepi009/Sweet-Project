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

        #endregion

        #region Public Methods

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
