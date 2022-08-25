using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MZ_Storage : MonoBehaviour
{
    public GameObject panel;
    public Mz_StorageUISlots slotPrefab;
    public Transform content;
    public GameObject goldInOutPanel;
    public Button buttonDeposit;
    public Button buttonWithdrawal;
    public InputField goldInputPanel;
    public Text goldTextPlaceholder;
    public Button buttonAction;
    public Text goldText;
    public Text goldInventoryText;
    [Header("[LABELS]")]
    public string depositLabel = "Deposit gold:";
    public string withdrawLabel = "Withdraw gold:";

    private ColorBlock btnDColor;
    private ColorBlock btnWColor;

    private int bdc = 0;
    private int bwc = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player player = Player.localPlayer;
        if (!player) return;

        // only update the panel if it's active
        if (panel.activeSelf)
        {
            // instantiate/destroy enough slots
            UIUtils.BalancePrefabs(slotPrefab.gameObject, player.MZ_Storage.Count, content);

            // refresh all items
            for (int i = 0; i < player.MZ_Storage.Count; ++i)
            {
                Mz_StorageUISlots slot = content.GetChild(i).GetComponent<Mz_StorageUISlots>();
                slot.dragAndDropable.name = i.ToString(); // drag and drop index
                ItemSlot itemSlot = player.MZ_Storage[i];

                if (itemSlot.amount > 0)
                {
                    slot.tooltip.enabled = true;
                    slot.tooltip.text = itemSlot.ToolTip();
                    slot.dragAndDropable.dragable = true;
                    slot.image.color = Color.white;
                    slot.image.sprite = itemSlot.item.image;
                    slot.amountOverlay.SetActive(itemSlot.amount > 1);
                    slot.amountText.text = itemSlot.amount.ToString();
                }
                else
                {
                    slot.button.onClick.RemoveAllListeners();
                    slot.tooltip.enabled = false;
                    slot.dragAndDropable.dragable = false;
                    slot.image.color = Color.clear;
                    slot.image.sprite = null;
                    slot.amountOverlay.SetActive(false);
                }
            }

            // ----- gold
            goldText.text = player.StorageGold.ToString();
            buttonDeposit.interactable = player.MZ_CanAccessStorage() && player.gold > 0 && player.MZ_HasEnoughGoldSpace();
            buttonWithdrawal.interactable = player.MZ_CanAccessStorage() && player.StorageGold > 0;
            goldInputPanel.interactable = player.MZ_CanAccessStorage() && (player.gold > 0 || player.StorageGold > 0);


            buttonDeposit.onClick.SetListener(() =>
            {
                if (goldInOutPanel.activeInHierarchy)
                {
                    goldInOutPanel.SetActive(false);
                }

                bdc = (bdc > 0 ? 0 : 1);
                bwc = 0;

                if (bdc == 1)
                {
                    goldTextPlaceholder.text = depositLabel;
                    goldInOutPanel.SetActive(true);
                    goldInputPanel.ActivateInputField();
                }
            });

            buttonWithdrawal.onClick.SetListener(() =>
            {
                if (goldInOutPanel.activeInHierarchy)
                {
                    goldInOutPanel.SetActive(false);
                }

                bwc = (bwc > 0 ? 0 : 1);
                bdc = 0;

                if (bwc == 1)
                {
                    goldTextPlaceholder.text = withdrawLabel;
                    goldInOutPanel.SetActive(true);
                    goldInputPanel.ActivateInputField();
                }
            });

            buttonAction.onClick.SetListener(() =>
            {
                string amountValue = goldInputPanel.text;

                if (!string.IsNullOrWhiteSpace(amountValue))
                {
                    int amount = int.Parse(amountValue);

                    if ((bdc != 1 || bwc != 1) && amount < 1) return;

                    if (bdc == 1)
                    {
                        player.Cmd_MZ_DepositGold(amount);
                    }

                    if (bwc == 1)
                    {
                        player.Cmd_MZ_WithdrawGold(amount);
                    }

                    goldText.text = player.StorageGold.ToString();
                    goldInventoryText.text = player.gold.ToString();

                    bdc = 0;
                    bwc = 0;
                }

                goldInputPanel.text = "";

                goldInOutPanel.SetActive(false);
            });
        }
        else
        {
            Hide();
        }
    }

    // -----------------------------------------------------------------------------------
    // Show
    // -----------------------------------------------------------------------------------
    public void Show()
    {
        Player player = Player.localPlayer;
        if (!player) return;

        if (!panel.activeSelf)
        {
            player.Cmd_MZ_LoadStorage();
            panel.SetActive(true);
        }
    }

    // -----------------------------------------------------------------------------------
    // Hide
    // -----------------------------------------------------------------------------------
    public void Hide()
    {
        Player player = Player.localPlayer;
        if (!player) return;

        if (panel.activeSelf)
        {
            player.Cmd_MZ_UnaccessStorage();
            panel.SetActive(false);
        }
    }

}
