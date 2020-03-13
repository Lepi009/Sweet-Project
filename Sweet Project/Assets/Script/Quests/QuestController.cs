using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public Quest currentQuest;

    public Quest firstQuest;

    private void Start()
    {
        currentQuest = new Quest_CollectCoin();
        currentQuest.Fire();
    }
}
