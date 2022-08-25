using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public partial class Player 
{
    [Server]
    public void QuestLearnSkill(Skill skill)
    {
        // call OnKilled in all active (not completed) quests
        for (int i = 0; i < quests.Count; ++i)
            if (!quests[i].completed)
                quests[i].OnLearned(this, i, skill);
    }

}
