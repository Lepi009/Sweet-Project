using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.LepiStudios.myQuestSystem
{

    public class Quest_CollectBanana : Quest_Collect
    {

        public override void Fire()
        {
            questName = QuestList.CollectBanana;

            base.Fire();

            this.SetDescribtion("Quest: Find the banana and collect it");

        }

        public override void Update()
        {
            //directly finish the quest whenever update is called because there is only one item to collect
            Finish();
        }

        public override void Finish()
        {
            base.Finish();

            this.SetDescribtion("You finished the Banana-Quest...\nBut know I need the fruits of the song PPAP");

            //all players are doing the quests together
            //that each player has his own quest, you need to have objects only on their game
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                if(player.GetComponent<QuestController>() != null) player.GetComponent<QuestController>().NextQuest();
            }
        }
    }
}
