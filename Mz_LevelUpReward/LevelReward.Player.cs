using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;
public partial class Player
{
    [Header("-=-=-=- MZ LEVEL UP Reward -=-=-=-")]
    [Tooltip("every Count level player will have reward")]
    public int Count_Level = 10;
    [Serializable]
    public partial struct RewardCommon
    {
        public int amount;
        public ScriptableItem defaultItem;
    }
    [Serializable]
    public partial struct RewardRare
    {
        public int amount;
        public int Level;
        public ScriptableItem[] defaultItemRare;
    }
    [Serializable]
    public partial struct itemReward
    {
        public RewardCommon[] defaultItem;
        public RewardRare[] ItemRare;
    }
    public itemReward rewardItem;
    [Server]
    [DevExtMethods("OnLevelUp")]
    private void OnLevelUp_Mz_Reward()
    {
        if (level % Count_Level == 0)
        {
            //set up reward here
            ScriptableItem item;
            foreach (var reward in rewardItem.defaultItem)
            {

                    ScriptableItem.dict.TryGetValue(reward.defaultItem.name.GetStableHashCode(), out item);
                    if (InventoryCanAdd(new Item(item), reward.amount))
                    {
                        InventoryAdd(new Item(item), reward.amount);
                    }
                
            }
            foreach (var reward in rewardItem.ItemRare)
            {
                if(level == reward.Level)
                {
                    for (int i = 0; i < reward.defaultItemRare.Length; i++)
                    {
                        ScriptableItem.dict.TryGetValue(reward.defaultItemRare[i].name.GetStableHashCode(), out item);
                        if (InventoryCanAdd(new Item(item), reward.amount))
                        {
                            InventoryAdd(new Item(item), reward.amount);
                        }
                    }

                }

            }
        }
 
    }

}
