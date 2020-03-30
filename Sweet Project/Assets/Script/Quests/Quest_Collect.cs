using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.LepiStudios.myQuestSystem { 

    /// <summary>
    /// this abstract class is subclass from Quest and is to organize all quest which task is to collect something
    /// </summary>
    public abstract class Quest_Collect : Quest
    {
        public override void Fire()
        {
            //updates all barricades
            QuestBarricade[] barricades = Object.FindObjectsOfType<QuestBarricade>();
            foreach(QuestBarricade barricade in barricades)
            {
                barricade.Proof(TimeOfQuest.Beginning, questName);
            }

            //updates all items
            QuestItem[] questItems = Object.FindObjectsOfType<QuestItem>();
            foreach (QuestItem item in questItems)
            {
                if (item.quest == questName)
                {
                    item.ShowItem();
                }
            }
        }

        public override void Finish()
        {
            //updates all barricades
            QuestBarricade[] barricades = Object.FindObjectsOfType<QuestBarricade>();
            foreach (QuestBarricade barricade in barricades)
            {
                barricade.Proof(TimeOfQuest.Ending, questName);
            }

        }
    }
}
