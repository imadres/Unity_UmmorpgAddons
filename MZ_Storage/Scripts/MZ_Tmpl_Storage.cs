using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MZ Storage", menuName = "UCE Templates/New MZ Storage", order = 999)]
public class MZ_Tmpl_Storage : ScriptableObject
{
    [Header("[STORAGE]")]
    public LinearInt StorageItems = new LinearInt { baseValue = 10 };

    public LinearLong StorageGold = new LinearLong { baseValue = 10000 };
    [Header("[ALLOWANCES]")]
    public bool storeSellable = true;

    public bool storeTradable = true;
    public bool storeDestroyable = true;

}
