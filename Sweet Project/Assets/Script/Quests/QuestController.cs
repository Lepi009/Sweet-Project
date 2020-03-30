using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.myQuestSystem
{
    ///<summary>component of the player, which controlls the different quests, fires the next and is the interface for quests</summary>
    public class QuestController : MonoBehaviour
    {
        [Tooltip("The current quest belonging to the player")]
        public Quest currentQuest;
    
        [Tooltip("The list of quest in its right order")]
        public LinkedList<Quest> questList = new LinkedList<Quest>();

        private void Start()
        {
            //initalize the quest list
            questList.AddLast(new Quest_CollectCoin());
            questList.AddLast(new Quest_CollectPPAP());

            //starts the first quest
            NextQuest();
        }
        
        ///<summary>method starts the next quest</summary>
        public void NextQuest()
        {
            currentQuest = questList.First.Value; //gets the next value(queue structure)
            questList.RemoveFirst(); //removes the first element
            currentQuest.Fire(); //starts the quest
        }
    }
}
