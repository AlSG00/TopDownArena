using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableApproachingHandler : MonoBehaviour
{
    // Обрабатывает, когда игрок приближается к интерактивному объекту
    // (например, когда он приближается к магазину, активируется подсветка)

    // У создаваемых аниматоров должны быть одинаковые свойства

    [SerializeField] private Animator _animator;
    [SerializeField] private float _deactivationDelay;

    private void OnTriggerEnter(Collider other)
    {
        // Активировать
    }

    private void OnTriggerExit(Collider other)
    {
        // Деактивировать
    }
}
