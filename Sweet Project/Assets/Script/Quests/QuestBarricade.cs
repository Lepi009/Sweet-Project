using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBarricade : MonoBehaviour
{
    [Tooltip("The quest where the barricade should disappear")]
    [SerializeField] private QuestList quest;

    [Tooltip("If the barricade should disappeat at the beginning or the end of the quest")]
    [SerializeField] private TimeOfQuest time;

    public void Proof(TimeOfQuest actualTime, QuestList quest)
    {
        Debug.Log(quest);
        if(actualTime == time && quest == this.quest)
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
