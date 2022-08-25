using UnityEngine;
using UnityEngine.UI;
public class MZ_UITools : MonoBehaviour
{
    public KeyCode hotKey = KeyCode.T;
    public GameObject panel;
    public UISkillSlot slotPrefab;
    public Transform content;

    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Player player = Player.localPlayer;
        if (!player) return;

        // hotkey (not while typing in chat, etc.)
        if (Input.GetKeyDown(hotKey) && !UIUtils.AnyInputActive())
            panel.SetActive(!panel.activeSelf);

        if (panel.activeSelf)
            Refresh();
    }
    private void Refresh()
    {
        Player player = Player.localPlayer;
        if (!player) return;

        // only update the panel if it's active
        if (panel.activeSelf)
        {
            // instantiate/destroy enough slots
            // (we only care about non status skills)
            UIUtils.BalancePrefabs(slotPrefab.gameObject, getSkillCount(), content);

            int t_index = -1;
            int s_index = 0;

            // refresh all
            for (int i = 0; i < player.skills.Count; ++i)
            {
                if (canShow(i))
                {
                    s_index = i;
                    t_index++;

                    UISkillSlot slot = content.GetChild(t_index).GetComponent<UISkillSlot>();
                    Skill skill = player.skills[s_index];

                    bool isPassive = skill.data is PassiveSkill;

                    // drag and drop name has to be the index in the real skill list,
                    // not in the filtered list, otherwise drag and drop may fail
                    int skillIndex = player.skills.FindIndex(s => s.name == skill.name);
                    slot.dragAndDropable.name = skillIndex.ToString();

                    // click event
                    slot.button.interactable = skill.level > 0 &&
                                               !isPassive &&
                                               player.CastCheckSelf(skill); // checks mana, cooldown etc.
                    slot.button.onClick.SetListener(() =>
                    {
                        player.CmdUseSkill(skillIndex);
                    });

                    // set state
                    slot.dragAndDropable.dragable = skill.level > 0 && !isPassive;

                    // image
                    if (skill.level > 0)
                    {
                        slot.image.color = Color.white;
                        slot.image.sprite = skill.image;
                    }
                }
            }
        }
    }
    private int getSkillCount()
    {
        Player player = Player.localPlayer;
        if (!player) return 0;

        int count = 0;

        for (int i = 0; i < player.skills.Count; ++i)
        {
            if (canShow(i))
                count++;
        }

        return count;
    }
    private bool canShow(int index)
    {
        Player player = Player.localPlayer;
        if (!player) return false;
        bool valid = player.skills[index].SkillTools && player.skills[index].unlearnable && player.skills[index].learnDefault;

        return valid;
    }
}

