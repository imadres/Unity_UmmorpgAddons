using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MZ_Shop : MonoBehaviour
{
    public KeyCode hotKey = KeyCode.Semicolon;
    public GameObject ShopPanel;
    public CanvasGroup group;
    public Button StartShop;
    public Button Cancel;
    public Button Buy;
    public Button Sell;
    [HideInInspector]public bool IsBuy =false;
    [HideInInspector]public bool IsSell =false;
    [HideInInspector]public bool isOK =false;


    void Update()
    {
        Player player = Player.localPlayer;
        if(player.inShopZone)
        {
            if(Input.GetKey(hotKey))
            {
                ShopPanel.SetActive(true);
                Buy.onClick.SetListener(()=>{
                    IsBuy = true;
                    IsSell = false;
                });
                Sell.onClick.SetListener(() => {
                    IsBuy = false;
                    IsSell = true;
                });
                StartShop.onClick.SetListener(() => {
                    group.interactable = false;
                    isOK = true;
                });
                Cancel.onClick.SetListener(() => {
                    group.interactable = true;
                    isOK = false;
                });
            }
        }
    }
}
