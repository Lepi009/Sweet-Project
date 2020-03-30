using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.myQuestSystem
{
    ///<summary>the class is a component for barricades which should disappear after a special quest</summary>
    public class QuestBarricade : MonoBehaviour
    {
        [Tooltip("The quest where the barricade should disappear")]
        [SerializeField] private QuestList quest;

        [Tooltip("At which time of the quest (begin or end) the barricade should disappear")]
        [SerializeField] private TimeOfQuest time;
        
        ///<summary> method proofs if the current quest is the one at which the barrricade should disappear</summary>
        ///<param name="currentTime">the current time of the quest</param>
        ///<param quest="currentQuest">the current quest that is running on the player</param>
        public void Proof(TimeOfQuest currentTime, QuestList currentQuest)
        {
            if(currentTime == this.time && currentQuest == this.quest)
            {
                Destroy(gameObject); //destroys the barricade when the quest and it's time matches to the conditions
            }
        }
    }
}
