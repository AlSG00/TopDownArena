using UnityEngine;

public interface IEquipable
{
    //public InventoryController.BindSlot[] availableSlots { get; set; }
    public void EquipModel(Transform itemSlotPivot);
    public void UnequipModel();
}
