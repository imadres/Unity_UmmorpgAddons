using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
[CreateAssetMenu(menuName = "uMMORPG Item/OtherItem", order = 999)]
public  class MZ_NewItem : UsableItem
{
    [Header("Other")]
    public int coins;
    public int Gold;

    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{COINS}",coins.ToString());
        tip.Replace("{GOLD}",Gold.ToString());
        return tip.ToString();
    }
    public override void Use(Player player, int inventoryIndex)
    {
        // always call base function too
        base.Use(player, inventoryIndex);
        player.coins += coins;
        player.gold += Gold;
        // decrease amount
        ItemSlot slot = player.inventory[inventoryIndex];
        slot.DecreaseAmount(1);
        player.inventory[inventoryIndex] = slot;
    }
}
