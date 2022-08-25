using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class Mz_QuestVisitNPC : ScriptableObject
{

    [Tooltip("Add Npc's from Prefabs [Limit 10]")]
    public Npc[] visitTarget;
    
}

public partial class Player
{
    public SyncListQuest MZ_quests = new SyncListQuest();
    [Client]
    public void MZ_IncreaseQuestNpcCounterFor(Npc npc)
    {
        for (int i = 0; i < MZ_quests.Count; ++i)
        {
            // active quest and not completed yet?
            if ((!MZ_quests[i].completed))
            {
                int index = i;
               // Cmd_MZ_IncreaseQuestNpcCounterFor(index, npc.name.GetDeterministicHashCode());
            }
        }
    }
   /* [Command]
    public void Cmd_MZ_IncreaseQuestNpcCounterFor(int index, int hash)
    {
        Quest quest = MZ_quests[index];
        bool bChanged = false;
        for (int j = 0; j < UCE_quests[index].visitTarget.Length; ++j)
        {
            if (MZ_quests[index].visitTarget[j].name.GetDeterministicHashCode() == hash &&
                quest.visitedTarget[j] != hash
                )
            {
                quest.visitedTarget[j] = hash;
                quest.visitedCount++;
                MZ_quests[index] = quest;
                bChanged = true;
                break;
            }
        }*/


    }

