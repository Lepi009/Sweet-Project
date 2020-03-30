using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.LepiStudios.myQuestSystem
{
    ///<summary>component of the player, which controlls the different quests, fires the next and is the interface for quests</summary>
    public class QuestController : MonoBehaviour
    {

        #region Private Serialized Fields

        [Tooltip("The current quest belonging to the player")]
        public Quest currentQuest;
    
        [Tooltip("The list of quest in its right order")]
        public LinkedList<Quest> questList = new LinkedList<Quest>();

        #endregion

        #region Private Fields

        [Tooltip("All important text components where the quest describtion should be presented")]
        private Text[] questTexts;

        #endregion

        private void Start()
        {
            if (!gameObject.GetComponent<PhotonView>().IsMine) Destroy(this);
            //find all quest text components to display the current issue
            GameObject[] gos = GameObject.FindGameObjectsWithTag("QuestText");
            questTexts = new Text[gos.Length + 1];
            Debug.Log(gos.Length);
            questTexts[0] = GameObject.FindObjectOfType<ListPlayer>().questDescribtion;
            for (int i = 1; i < gos.Length + 1; i++)
            {
                questTexts[i] = gos[i-1].GetComponent<Text>();
            }

            //initalize the quest list
            questList.AddLast(new Quest_CollectBanana());
            questList.AddLast(new Quest_CollectPPAP());

            //starts the first quest
            NextQuest();
        }
        
        ///<summary>method starts the next quest</summary>
        public void NextQuest()
        {
            currentQuest = questList.First.Value; //gets the next value(queue structure)

            foreach(Text t in questTexts)
            {
                currentQuest.OnChangeDescribtion += t.SetText;
            }

            questList.RemoveFirst(); //removes the first element
            currentQuest.Fire(); //starts the quest
        }
    }
}
