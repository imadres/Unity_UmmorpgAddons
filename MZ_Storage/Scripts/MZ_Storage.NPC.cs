using Mirror;
using UnityEngine;

public partial class Npc 
{
    [Header("[-=-=-=- MZ Storage -=-=-=-]")]
    public bool offersStorage = false;
    [HideInInspector] public SyncListString StorageBusy = new SyncListString();
    public bool checStorageAccess(Player player)
    {
        if (!offersStorage)
        { return false;
        }
        else
        { return true;
        }
    }
}
