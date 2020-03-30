using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.myQuestSystem
{
    ///<summary>component of all items which are relevant for quests</summary>
    public class QuestItem : MonoBehaviour
    {
        #region Private Serialized Fields
        
        [Tooltip("Defines if the item is visible and relevant at the beginning or should be enabled when the player starts the refering quest")]
        public bool enabledAtTheBeginning = true;
        
        [Tooltip("The quest, which is important for the item")]
        public QuestList quest;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            if(!enabledAtTheBeginning) SetActiveItem(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponent<QuestController>() == null) return;
                Quest currentQuest = other.gameObject.GetComponent<QuestController>().currentQuest;

                if(currentQuest.questName == quest)
                {
                    currentQuest.Update();
                    Destroy(gameObject);
                }

            }
        }
        
        #endregion
        
        #region Public Methods
        
        public void ShowItem()
        {
            if(!enabledAtTheBeginning) SetActiveItem(true);
        }
        
        #endregion
        
        #region Private Methods
        
        private void SetActiveItem(bool enable) {
            GetComponent<SpriteRenderer>().enabled = enable;
            GetComponent<BoxCollider2D>().enabled = enable;
        }
        
        #endregion
    }
}
