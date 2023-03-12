using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSwapper : MonoBehaviour
{
    private TerrainChecker checker;
    private string currentLayer;
    private Player_Movement _playerMovement;
    public FootstepCollection[] terrainFootstepCollections;


    private void Start()
    {
        checker = new TerrainChecker();
        _playerMovement = GetComponent<Player_Movement>();
    }

    public void CheckLayers(LayerMask activeLayers)
    {
        // рейкастим вниз
        // смотрим, существует ли террейн
        
        RaycastHit _hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, out _hit, 5, activeLayers))
        {
            if (_hit.transform.GetComponent<Terrain>() != null)
            {
                Terrain terrain = _hit.transform.GetComponent<Terrain>();

                if (currentLayer != checker.GetLayerName(transform.position, terrain))
                {
                    currentLayer = checker.GetLayerName(transform.position, terrain);

                    for (int i = 0; i < terrainFootstepCollections.Length; i++)
                    {
                        if (currentLayer == terrainFootstepCollections[i].name)
                        {
                            _playerMovement.SwapFootsteps(terrainFootstepCollections[i]);
                        }
                    }
                }
            }
            if (_hit.transform.GetComponent<SurfaceType>() != null)
            {
                FootstepCollection collection = _hit.transform.GetComponent<SurfaceType>().footstepCollection;
                currentLayer = collection.name;
                _playerMovement.SwapFootsteps(collection);
            }
        }
    }
}
