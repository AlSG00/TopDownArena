using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableObject : MonoBehaviour, SCRIPT_IInteractable
{
    public bool canInteract { get; set; }
    public bool alreadyInteracting { get; set; }
    public bool inInteractionArea { get; set; }

    [Range(1, 999)] public int stackCount;

    [Header("References")]
    public SCRIPT_InventoryItem inventoryItem;


    public delegate int PickAction(SCRIPT_InventoryItem item, int stackCount);
    public static event PickAction OnItemPick;

    private void Awake()
    {
        if (stackCount > inventoryItem.maxStackCount)
        {
            stackCount = inventoryItem.maxStackCount;
        }
        else if (stackCount == 0)
        {
            stackCount = 1;
        }
    }

    private void Start()
    {
        alreadyInteracting = false;
        canInteract = false;
        inInteractionArea = false;
        //inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
        //inventory = GameObject.Find("_PlayerCamera").GetComponent<InventoryController>();
        //pickUpAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    }

    public void Interact()
    {
        //canInteract = false;
        if (OnItemPick != null)
        {
            stackCount = OnItemPick.Invoke(inventoryItem, stackCount);

            if (stackCount == 0)
            {
                Destroy(gameObject);
            }
        }
        

        //inventory.selectedItemGrid = inventory.inventoryGrid;
        //int stackCountRemaining = inventory.InsertIntoAvailableStacks(inventoryItem, stackCount, true);
        //if (stackCountRemaining > 0)
        //{
        //    Vector2Int? positionOnGrid = inventory.selectedItemGrid.FindSpaceForObject(inventoryItem);
        //    if (positionOnGrid == null)
        //    {
        //        stackCount = stackCountRemaining;
        //        alreadyInteracting = false;
        //        return;
        //    }

        //    //inventoryItem.stackCount = stackCountRemaining;
        //    inventory.InsertItemIntoInventory(inventoryItem, stackCountRemaining);
        //    //PlayPickUpAudio();
        //    //Destroy(gameObject);
        //}
        ////Сделать ивент, чтобы проигрывался звук подбора предметов в инвентаре.
        ////Проигрываться будет через (2 варианта):
        ////    - аудио в инвентаре (здесь ивент, в инвентаре подписка на ивент)
        ////    - аудио в отдельном компоненте для обработки звука персонажа, мол, инвентарь, фонарик, пожрать и т.д. (здесь ивент, в компоненте подписка на ивент)

        //Destroy(gameObject);
        //else
        //{
        //    //PlayPickUpAudio();
        //    Destroy(gameObject);
        //}
    }

    //private void PlayPickUpAudio()
    //{
    //    if (pickUpAudioSource != null &&
    //        inventoryItem.pickFromGroundAudio != null)
    //    {
    //        pickUpAudioSource.PlayOneShot(inventoryItem.pickFromGroundAudio);
    //    }
    //}
}
