using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Quest currentQuest = other.gameObject.GetComponent<QuestController>().currentQuest;
            if(currentQuest is Quest_CollectCoin)
            {
                ((Quest_CollectCoin)currentQuest).Finish();
                Destroy(gameObject);
            }
        }
    }
}
