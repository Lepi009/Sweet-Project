﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.LepiStudios.myQuestSystem
{

    public class Quest_CollectCoin : Quest_Collect
    {
        private GameObject player;

        private Text questText;

        public override void Fire()
        {
            questName = QuestList.CollectBanana;

            base.Fire();
            questText = GameObject.FindGameObjectWithTag("QuestText").GetComponent<Text>();
            questText.text = "Quest: Find the banana and collect it";
        }

        public override void Update()
        {
            Finish();
        }

        public override void Finish()
        {
            base.Finish();
            questText.text = "You finished the Banana-Quest...\nBut know I need the fruits of the song PPAP";

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            //all players are doing the quests together
            //that each player has his own quest, you need to have objects only on their game
            foreach(GameObject player in players)
            {
                player.GetComponent<QuestController>().NextQuest();
            }
        }
    }
}
