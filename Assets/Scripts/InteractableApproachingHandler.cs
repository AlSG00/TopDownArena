using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableApproachingHandler : MonoBehaviour
{

    // Обрабатывает, когда игрок приближается к интерактивному объекту
    // (например, когда он приближается к магазину, активируется подсветка)

    // У создаваемых аниматоров должны быть одинаковые свойства

    [SerializeField] private Animator _animator; // TODO: Запрогать
    [SerializeField] private float _deactivationDelay; // TODO: Запрогать

    private void OnTriggerEnter(Collider other)
    {
        // Активировать
        Debug.Log("Entered door area");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaved door area");
        // Деактивировать
    }
}
