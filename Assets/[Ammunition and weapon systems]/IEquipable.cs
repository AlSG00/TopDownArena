using UnityEngine;

public interface IEquipable
{
    //public InventoryController.BindSlot[] availableSlots { get; set; }
    public void EquipModel(Transform itemSlotPivot, string slotName);
    public void UnequipModel();
}
