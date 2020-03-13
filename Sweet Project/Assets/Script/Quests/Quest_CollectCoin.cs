using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_CollectCoin : Quest
{
    private GameObject banana;

    private GameObject player;

    public override void Fire()
    {
        Debug.Log("Collect the banana");
    }

    public void Finish()
    {
        Debug.Log("The banana has been collected");
    }

    



}
