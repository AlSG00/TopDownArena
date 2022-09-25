using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_SpawnEnemies : MonoBehaviour
{
    private bool _isFinished = false;
    public List<Transform> spawnpoint;
    public GameObject enemyPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isFinished && spawnpoint != null && spawnpoint.Count > 0)
        {
            for (int i = 0; i < spawnpoint.Count; i++)
            {
                Instantiate(enemyPrefab, spawnpoint[i].position, Quaternion.identity);
            }
            _isFinished = true;
        }
    }
}
