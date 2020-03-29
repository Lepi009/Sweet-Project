using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public Quest currentQuest;

    public Quest firstQuest;

    public LinkedList<Quest> questList = new LinkedList<Quest>();

    private void Start()
    {
        questList.AddLast(new Quest_CollectCoin());
        questList.AddLast(new Quest_CollectPPAP());


        currentQuest = questList.First.Value;
        questList.RemoveFirst();
        currentQuest.Fire();
    }

    public void NextQuest()
    {
        currentQuest = questList.First.Value;
        questList.RemoveFirst();
        currentQuest.Fire();
    }
}
