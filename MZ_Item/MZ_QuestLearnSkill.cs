using UnityEngine;
using System.Text;
[CreateAssetMenu(menuName = "uMMORPG Quest/Learn Skill", order = 999)]
public  class MZ_QuestLearnSkill: ScriptableQuest
{
    [Header("Skill Learned")]
    public ScriptableSkill SkillLearn;
    public int SkillLevelRequired;
    public override void OnLearned(Player player, int questIndex,Skill skillindex)
    {
        Quest quest = player.quests[questIndex];
        if (quest.progress < SkillLevelRequired)
        {
            // increase int field in quest (up to 'amount')
            ++quest.progress;
            player.quests[questIndex] = quest;
        }
    }
    public override bool IsFulfilled(Player player, Quest quest)
    {
        return quest.progress >= SkillLevelRequired;
    }

    public override string ToolTip(Player player, Quest quest)
    {
        // we use a StringBuilder so that addons can modify tooltips later too
        // ('string' itself can't be passed as a mutable object)
        StringBuilder tip = new StringBuilder(base.ToolTip(player, quest));
        tip.Replace("{SKILLTARGET}",SkillLearn.name);
        return tip.ToString();
    }
}
