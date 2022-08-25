using UnityEngine;
using UnityEngine.UI;

public class MZ_UI_StorageNPCDialogue : MonoBehaviour
{
    public GameObject npcDialoguePanel;
    public Button StorageButton;
    public MZ_Storage StoragePanel;
    public GameObject inventoryPanel;

    private bool init = false;

    // -----------------------------------------------------------------------------------
    // Update
    // -----------------------------------------------------------------------------------
    private void Update()
    {
        Player player = Player.localPlayer;
        if (!player) return;

        if (player.target != null &&
            player.target is Npc &&
            Utils.FindDistance(player.collider, player.target.collider) <= player.interactionRange)
        {
            Init();

            Npc npc = (Npc)player.target;

            StorageButton.gameObject.SetActive(npc.checStorageAccess(player));
            StorageButton.interactable = true;

            if (StorageButton.interactable)
            {
                StorageButton.onClick.SetListener(() =>
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    StoragePanel.Show();
                    inventoryPanel.SetActive(true);
                });
            }
        }
        else
        {
            Reset();
            StoragePanel.Hide();
        }
    }

    // -----------------------------------------------------------------------------------
    // Init
    // -----------------------------------------------------------------------------------
    private void Init()
    {
        if (init) return;

        Player player = Player.localPlayer;
        if (!player) return;

        init = true;

        Npc npc = (Npc)player.target;
    }

    // -----------------------------------------------------------------------------------
    // Reset
    // -----------------------------------------------------------------------------------
    private void Reset()
    {
        if (!init) return;

        init = false;
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================