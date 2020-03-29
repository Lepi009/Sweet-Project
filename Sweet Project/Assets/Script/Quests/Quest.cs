using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest
{
    public QuestList questName;
    public abstract void Fire();

    public abstract void Update();

    public abstract void Finish();
}
