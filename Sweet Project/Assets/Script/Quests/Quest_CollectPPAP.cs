using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest_CollectPPAP : Quest_Collect
{
    ///<summary>var safes the "quest board", which shows the current running quest</summary>
    private Text questText;
    
    ///<summary>var counts the founded items</summary>
    public int foundItems = 0;

    public override void Fire()
    {
        questName = QuestList.CollectPPAP;
        
        base.Fire();
        
        questText = GameObject.FindGameObjectWithTag("QuestText").GetComponent<Text>();

    }

    public override void Update()
    {
        //method called whenever a fruit is found
        foundItems++;
        
        if(foundItems == 2) //called when the second fruit was found
        {
            Finish();
        }
    }

    public override void Finish()
    {
        base.Finish();
        
        questText.text = "You finished the PPAP quest...now sing for me\nAnd maybe there is now a way free that was not free yet";
    }

    



}
