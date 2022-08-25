using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
public partial class Player
{
    [Header(" MZ Item Drop Button Collect")]
    public KeyCode hotKey = KeyCode.Space;
    // Update is called once per frame
   /* UCE_ItemDrop[] itemDrop;
    private void Update()
    {
        if(Input.GetKey(hotKey))
        {
            itemDrop = GameObject.FindObjectsOfType<UCE_ItemDrop>();
            if(itemDrop !=null)
            {
                foreach (var itemN in itemDrop)
                {
                    if(interactionRange <= Utils.FindDistance(this.collider, itemN.collider))
                    {
                        OnCollectItemServer(this, itemN);
                    }
                }
            }
        }
    }
    [ServerCallback]
    public void OnCollectItemServer(Player player,UCE_ItemDrop item)
    {
        if ((item.amount > 0 || item.gold > 0) && player.InventoryCanAdd(item.item, item.amount)
#if _iMMOLOOTRULES && _iMMOITEMDROP
            && UCE_ValidateTaggedLooting(player)
#endif

           )
        {
            if (item.gold > 0 || player.InventoryAdd(item.item, item.amount))
            {
                player.gold += item.gold;

                if (item.amount > 0)
                    player.UCE_TargetAddMessage(item.takeMessage + item.item.name);

                if (item.gold > 0)
                    player.UCE_TargetAddMessage(item.goldMessage + item.gold);

                // clear drop's item slot too so it can't be looted again
                // before truly destroyed
                item.gold = 0;
                item.amount = 0;

                NetworkServer.Destroy(item.gameObject);
            }
        }
        else
        {
            player.UCE_TargetAddMessage(item.failMessage);
        }
    }*/
}
