using System;

#if _MYSQL && _SERVER
using MySql.Data;
using MySql.Data.MySqlClient;
#elif _SQLITE && _SERVER
using SQLite;
using UnityEngine;
#endif

public partial class Database 
{
#if _SQLITE && _SERVER
    class MZ_Storage
    {
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        public string account { get; set; }
        public long gold { get; set; }
    }
    class MZ_Storage_items
    {
        public string account { get; set; }
        public int slot { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public int summonedHealth { get; set; }
        public int summonedLevel { get; set; }
        public long summonedExperience { get; set; }
    }
#endif
    [DevExtMethods("Connect")]
    private void Connect_MZ_Storage()
    {
        //storage Addon 
        connection.CreateTable<MZ_Storage>();
        connection.CreateTable<MZ_Storage_items>();
        connection.CreateIndex(nameof(MZ_Storage_items), new[] { "account", "slot" });
    }

    public void MZ_LoadStorage(Player player)
    {

       player.resetStorage();
        var MZ_StorageData = connection.FindWithQuery<MZ_Storage>("SELECT gold FROM MZ_Storage WHERE account=?", player.account);

        // -- exists already? load to player 
        if (MZ_StorageData != null)
        {
            player.StorageGold = MZ_StorageData.gold;
            Debug.Log("Load stroge success");
            
        }else
        {
            connection.InsertOrReplace(new MZ_Storage { account = player.account, gold = player.StorageGold  });
            player.StorageGold = 0;
            Debug.Log("Load stroge Faild");
        }

       for (int i = 0; i < player.StorageItems; ++i)
       {
            player.MZ_Storage.Add(new ItemSlot());
       }
        var table = connection.Query<MZ_Storage_items>("SELECT * FROM MZ_Storage_items WHERE account=?", player.account);
       foreach (MZ_Storage_items row in table)
       {
            if (row.slot < player.StorageItems)
            {
                if (ScriptableItem.dict.TryGetValue(row.name.GetStableHashCode(), out ScriptableItem template))
                {
                    Item item = new Item(template);
                    int amount = row.amount;
                    item.summonedHealth = row.summonedHealth;
                    item.summonedLevel = row.summonedLevel;
                    item.summonedExperience = row.summonedExperience;
                    player.MZ_Storage[row.slot] = new ItemSlot(item, amount);
                }
                else Debug.LogWarning("LoadStorage: skipped item " + row.name + " for " + player.account + " because it doesn't exist anymore. If it wasn't removed intentionally then make sure it's in the Resources folder.");
            }
            else Debug.LogWarning("LoadStorage: skipped slot " + row.slot + " for " + player.account + " because it's bigger than size " + player.StorageItems);
        }
    }

    [DevExtMethods("CharacterSave")]
    private void CharacterSave_MZ_Storage(Player player)
    {
#if _SERVER
        MZ_SaveStorage(player);
#endif
    }

    public void MZ_SaveStorage(Player player)
    {
#if _SERVER && _SQLITE
        connection.Execute("DELETE FROM MZ_Storage_items WHERE account=?", player.account);  

            for (int i = 0; i < player.MZ_Storage.Count; ++i)
            {
                ItemSlot slot = player.MZ_Storage[i];

                if (slot.amount > 0)
                {
                    connection.InsertOrReplace(new MZ_Storage_items
                    {
                        account = player.account,
                        slot = i,
                        name = slot.item.name,
                        amount = slot.amount,
                        summonedHealth = slot.item.summonedHealth,
                        summonedLevel = slot.item.summonedLevel,
                        summonedExperience = slot.item.summonedExperience
                    });
                }
            }
        connection.InsertOrReplace(new MZ_Storage { account = player.account, gold = player.StorageGold });
#endif
    }

}
