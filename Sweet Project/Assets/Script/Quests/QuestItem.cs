using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public bool enabledAtTheBeginning = true;

    public QuestList quest;

    private void Start()
    {
        if (!enabledAtTheBeginning) GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ShowItem()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GetComponent<SpriteRenderer>().enabled == false) return;
        if(other.gameObject.tag == "Player")
        {
            Quest currentQuest = other.gameObject.GetComponent<QuestController>().currentQuest;

            if(currentQuest.questName == quest)
            {
                currentQuest.Update();
                Destroy(gameObject);
            }
        }
    }
}
