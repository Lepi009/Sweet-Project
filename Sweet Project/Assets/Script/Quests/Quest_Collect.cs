using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest_Collect : Quest
{
    public override void Fire()
    {
        //updates all barricades
        QuestBarricade[] barricades = Object.FindObjectsOfType<QuestBarricade>();
        foreach(QuestBarricade barricade in barricades)
        {
            barricade.Proof(TimeOfQuest.Beginning, questName);
        }

        //updates all items
        QuestItem[] questItems = Object.FindObjectsOfType<QuestItem>();
        foreach (QuestItem item in questItems)
        {
            if (item.quest == questName)
            {
                item.ShowItem();
            }
        }
    }

    public override void Finish()
    {
        //updates all barricades
        QuestBarricade[] barricades = Object.FindObjectsOfType<QuestBarricade>();
        foreach (QuestBarricade barricade in barricades)
        {
            barricade.Proof(TimeOfQuest.Ending, questName);
        }

    }
}
