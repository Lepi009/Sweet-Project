using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.LepiStudios.myQuestSystem
{

    public class Quest_CollectPPAP : Quest_Collect
    {
        ///<summary>var counts the founded items</summary>
        public int foundItems = 0;

        public override void Fire()
        {
            questName = QuestList.CollectPPAP;

            base.Fire();

        }

        public override void Update()
        {
            //method called whenever a fruit is found
            foundItems++;

            if (foundItems == 2) //called when the second fruit was found
            {
                Finish();
            }
        }

        public override void Finish()
        {
            base.Finish();
            this.SetDescribtion("You finished the PPAP quest...now sing for me\nAnd maybe there is now a way free that was not free yet");
        }
    }
}
