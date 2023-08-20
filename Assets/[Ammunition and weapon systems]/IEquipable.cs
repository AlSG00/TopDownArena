using UnityEngine;

public interface IEquipable
{
    public void EquipModel(Transform itemSlotPivot);
    public void UnequipModel();
}
