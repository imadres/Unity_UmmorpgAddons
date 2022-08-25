using Mirror;
using System;
using System.Linq;
using UnityEngine;

public partial class Player
{
    public SyncListItemSlot MZ_Storage = new SyncListItemSlot();

    [Header("[-=-=-=- MZ Storage -=-=-=-]")]
     public MZ_Tmpl_Storage Storage;

    [SyncVar] private long _StorageGold = 0;
    [SyncVar, HideInInspector] public bool StorageLock = false;
    [SyncVar, HideInInspector] public bool StorageActionDone = false;
    private Npc selectedStorage;
    [DevExtMethods("Awake")]
    void OnCreateCharacterLoad()
    {
        Cmd_MZ_LoadStorage();
    }
    public void resetStorage()
    {
        MZ_Storage.Clear();
        _StorageGold = 0;
    }
    public long StorageGold
    {
        get { return _StorageGold; }
        set { _StorageGold = Math.Max(value, 0); }
    }
    public int StorageItems
    {
        get
        {
            if (Storage)
            {
                return Storage.StorageItems.Get(0);
            }
            else
            {
                Debug.LogWarning("You forgot to assign a guild warehouse template to one of your player prefabs");
                return 0;
            }
        }
    }
    public long GetStorageGold
    {
        get
        {
            if (Storage)
            {
                return Storage.StorageGold.Get(1);
            }
            else
            {
                Debug.LogWarning("You forgot to assign a guild warehouse template to one of your player prefabs");
                return 0;
            }
       }
    }
    public int GetMZ_StorageIndexByName(string itemName)
    {
        return MZ_Storage.FindIndex(slot => slot.amount > 0 && slot.item.name == itemName);
    }
    public int MZ_StorageSlotsFree()
    {
        return MZ_Storage.Count(slot => slot.amount == 0);
    }
    public int Mz_StorageCountAmount(Item item)
    {
        return (from slot in MZ_Storage
                where slot.amount > 0 && slot.item.Equals(item)
                select slot.amount).Sum();
    }
    public bool Mz_StorageRemoveAmount(Item item, int amount)
    {
        for (int i = 0; i < MZ_Storage.Count; ++i)
        {
            ItemSlot slot = MZ_Storage[i];

            if (slot.amount > 0 && slot.item.Equals(item))
            {
                amount -= slot.DecreaseAmount(amount);
                MZ_Storage[i] = slot;
                if (amount == 0) return true;
            }
        }

        return false;
    }
    public bool Mz_StorageCanAddAmount(Item item, int amount)
    {
        for (int i = 0; i < MZ_Storage.Count; ++i)
        {
            if (MZ_Storage[i].amount == 0)
                amount -= item.maxStack;
            else if (MZ_Storage[i].item.Equals(item))
                amount -= (MZ_Storage[i].item.maxStack - MZ_Storage[i].amount);
            if (amount <= 0) return true;
        }

        return false;
    }
    public bool Mz_StorageAddAmount(Item item, int amount)
    {
        if (item.tradable && Mz_StorageCanAddAmount(item, amount))
        {
            // go through each slot
            for (int i = 0; i < MZ_Storage.Count; ++i)
            {
                // empty? then fill slot with as many as possible
                if (MZ_Storage[i].amount == 0)
                {
                    int add = Mathf.Min(amount, item.maxStack);
                    MZ_Storage[i] = new ItemSlot(item, add);
                    amount -= add;

                }
                // not empty and same type? then add free amount (max-amount)
                else if (MZ_Storage[i].item.Equals(item))
                {
                    ItemSlot temp = MZ_Storage[i];
                    amount -= temp.IncreaseAmount(amount);
                    MZ_Storage[i] = temp;
                }

                // were we able to fit the whole amount already?
                if (amount <= 0) return true;
            }
        }
        return false;
    }

    [Command]
    public void Cmd_MZ_BusyStorage(GameObject selectedNpc, string guild)
    {
        Npc npc = selectedNpc.GetComponent<Npc>();
        if (npc != null)
        {
            selectedStorage = npc;
        }
    }
    [Command]
    public void Cmd_MZ_LoadStorage()
    {
        Database.singleton.MZ_LoadStorage(this);
    }
    [Command]
    public void Cmd_MZ_SaveStorage()
    {
        Database.singleton.MZ_SaveStorage(this);
    }

    [Command]
    public void CmdSwapInventoryMZ_Storage(int fromIndex, int toIndex)
    {
        ItemSlot slot = inventory[fromIndex];

        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            (Storage.storeTradable || (!Storage.storeTradable && !slot.item.tradable)) &&
            (Storage.storeSellable || (!Storage.storeSellable && !slot.item.sellable)) &&
            (Storage.storeDestroyable || (!Storage.storeDestroyable && !slot.item.destroyable)) &&
            0 <= fromIndex && fromIndex < inventory.Count &&
            0 <= toIndex && toIndex < MZ_Storage.Count)
        {
            // don't allow player to add items which has zero amount or if it's summoned pet item
            if (slot.amount > 0 && !slot.item.summoned)
            {
                // swap them
                inventory[fromIndex] = MZ_Storage[toIndex];
                MZ_Storage[toIndex] = slot;
                Cmd_MZ_SaveStorage();
                StorageActionDone = true;
            }
        }
    }
    [Command]
    public void CmdSwapMZ_storageInventory(int fromIndex, int toIndex)
    {
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= fromIndex && fromIndex < MZ_Storage.Count &&
            0 <= toIndex && toIndex < inventory.Count)
        {
            // swap them
            ItemSlot temp = MZ_Storage[fromIndex];
            MZ_Storage[fromIndex] = inventory[toIndex];
            inventory[toIndex] = temp;
            StorageActionDone = true;
            Cmd_MZ_SaveStorage();
        }
    }
    [Command]
    public void CmdSwapMZ_StorageMZ_Storage(int fromIndex, int toIndex)
    {
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= fromIndex && fromIndex < MZ_Storage.Count &&
            0 <= toIndex && toIndex < MZ_Storage.Count &&
            fromIndex != toIndex)
        {
            // swap them
            ItemSlot temp = MZ_Storage[fromIndex];
            MZ_Storage[fromIndex] = MZ_Storage[toIndex];
            MZ_Storage[toIndex] = temp;
            StorageActionDone = true;
            Cmd_MZ_SaveStorage();
        }
    }
    [Command]
    public void CmdMZ_StorageSplit(int fromIndex, int toIndex)
    {
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= fromIndex && fromIndex < MZ_Storage.Count &&
            0 <= toIndex && toIndex < MZ_Storage.Count &&
            fromIndex != toIndex)
        {
            // slotFrom has to have an entry, slotTo has to be empty
            ItemSlot slotFrom = MZ_Storage[fromIndex];
            ItemSlot slotTo = MZ_Storage[toIndex];

            // from entry needs at least amount of 2
            if (slotFrom.amount >= 2 && slotTo.amount == 0)
            {
                // split them serversided (has to work for even and odd)
                slotTo = slotFrom;

                slotTo.amount = slotFrom.amount / 2;
                slotFrom.amount -= slotTo.amount; // works for odd too

                // put back into the list
                MZ_Storage[fromIndex] = slotFrom;
                MZ_Storage[toIndex] = slotTo;
                StorageActionDone = true;
                Cmd_MZ_SaveStorage();
            }
        }
    }
    [Command]
    public void CmdMZ_StorageMerge(int fromIndex, int toIndex)
    {
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= fromIndex && fromIndex < MZ_Storage.Count &&
            0 <= toIndex && toIndex < MZ_Storage.Count &&
            fromIndex != toIndex)
        {
            // both items have to be valid
            ItemSlot slotFrom = MZ_Storage[fromIndex];
            ItemSlot slotTo = MZ_Storage[toIndex];

            if (slotFrom.amount > 0 && slotTo.amount > 0)
            {
                // check if the both items are the same type
                if (slotFrom.item.Equals(slotTo.item))
                {
                    int put = slotTo.IncreaseAmount(slotFrom.amount);
                    slotFrom.DecreaseAmount(put);

                    // put back into the list
                    MZ_Storage[fromIndex] = slotFrom;
                    MZ_Storage[toIndex] = slotTo;
                    StorageActionDone = true;
                    Cmd_MZ_SaveStorage();
                }
            }
        }
    }
    [Command]
    public void Cmd_MZ_DepositGold(int amount)
    {
        Mz_StorageAddGold(amount);
    }

    // -----------------------------------------------------------------------------------
    //
    // -----------------------------------------------------------------------------------
    [Command]
    public void Cmd_MZ_WithdrawGold(int amount)
    {
        Mz_StorageRemoveGold(amount);
        Cmd_MZ_SaveStorage();
    }
    [Server]
    public void Mz_StorageAddGold(long amount)
    {
        if (MZ_HasEnoughGoldOnInventory(amount) && MZ_HasEnoughGoldSpace(amount))
        {
            gold -= amount;
            StorageGold += amount;
            StorageActionDone = true;
            Cmd_MZ_SaveStorage();
        }
    }

    [Server]
    public void Mz_StorageRemoveGold(long amount)
    {
        if (HasEnoughGoldOnMZ_Storage(amount))
        {
            StorageGold -= amount;
            gold += amount;
            StorageActionDone = true;
            Cmd_MZ_SaveStorage();
        }
    }
    [Server]
    public bool HasEnoughGoldOnMZ_Storage(long amount)
    {
        return amount > 0 && StorageGold >= amount;
    }

    public bool MZ_HasEnoughGoldOnInventory(long amount)
    {
        return amount > 0 && gold >= amount;
    }
    public bool MZ_HasEnoughGoldSpace(long amount = 1)
    {
        return amount > 0 && GetStorageGold >= StorageGold + amount;
    }
    public bool MZ_CanAccessStorage()
    {
        return (isAlive &&
                (!StorageLock ||
                (StorageLock && InGuild() && guild.CanNotify(name)))
                );
    }
    [DevExtMethods("OnDragAndDrop")]
    private void OnDragAndDrop_MZ_StorageSlot_MZ_StorageSlot(int[] slotIndices)
    {
        // merge? (just check the name, rest is done server sided)
        if (MZ_Storage[slotIndices[0]].amount > 0 && MZ_Storage[slotIndices[1]].amount > 0 &&
            MZ_Storage[slotIndices[0]].item.Equals(MZ_Storage[slotIndices[1]].item))
        {
            CmdMZ_StorageMerge(slotIndices[0], slotIndices[1]);
            // split?
        }
        else if (Utils.AnyKeyPressed(inventorySplitKeys))
        {
            CmdMZ_StorageSplit(slotIndices[0], slotIndices[1]);
            // swap?
        }
        else
        {
            CmdSwapMZ_StorageMZ_Storage(slotIndices[0], slotIndices[1]);
        }
    }
    [DevExtMethods("OnDragAndDrop")]
    private void OnDragAndDrop_MZ_StorageSlot_InventorySlot(int[] slotIndices)
    {
        if (MZ_Storage[slotIndices[0]].amount > 0)
        {
            CmdSwapMZ_storageInventory(slotIndices[0], slotIndices[1]);
        }
    }
    [DevExtMethods("OnDragAndDrop")]
    private void OnDragAndDrop_InventorySlot_MZ_StorageSlot(int[] slotIndices)
    {
        if (inventory[slotIndices[0]].amount > 0)
        {
            CmdSwapInventoryMZ_Storage(slotIndices[0], slotIndices[1]);
        }
    }
    [Command]
    public void Cmd_MZ_UnaccessStorage()
    {
        if (selectedStorage != null)
        {
            Cmd_MZ_SaveStorage();
            selectedStorage = null;
        }
    }
    [DevExtMethods("OnDestroy")]
    private void OnDestroy_MZ_Storage()
    {
        if (NetworkServer.active)
        {
               Cmd_MZ_UnaccessStorage();
        }
    }
}
