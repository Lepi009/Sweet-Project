using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest_CollectPPAP : Quest_Collect
{
    private GameObject banana;

    private GameObject player;

    private Text questText;

    public int foundItems = 0;

    public override void Fire()
    {
        questName = QuestList.CollectPPAP;
        base.Fire();
        questText = GameObject.FindGameObjectWithTag("QuestText").GetComponent<Text>();

    }

    public override void Update()
    {
        if(foundItems == 1)
        {
            Finish();
        } else
        {
            foundItems++;
        }
    }

    public override void Finish()
    {
        base.Finish();
        questText.text = "You finished the PPAP quest...now sing for me\nAnd maybe there is now a way free that was not free yet";
    }

    



}
