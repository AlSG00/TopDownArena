using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChecker : MonoBehaviour
{
    private float[] GetTextureMix(Vector3 playerPosition, Terrain terrain)
    {
        Vector3 terrainPosition = terrain.transform.position;
        TerrainData terrainData = terrain.terrainData;
        int mapX = Mathf.RoundToInt((playerPosition.x - terrainPosition.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((playerPosition.z - terrainPosition.z) / terrainData.size.z * terrainData.alphamapHeight);
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellmix = new float[splatmapData.GetUpperBound(2) + 1];
        for (int i = 0; i < cellmix.Length; i++)
        {
            cellmix[i] = splatmapData[0, 0, i];
        }
        return cellmix;
    }

    public string GetLayerName(Vector3 playerPosition, Terrain terrain)
    {
        float[] cellMix = GetTextureMix(playerPosition, terrain);
        float strongest = 0;
        int maxIndex = 0;

        for (int i = 0; i < cellMix.Length; i++)
        {
            if (cellMix[i] > strongest)
            {
                maxIndex = i;
                strongest = cellMix[i];
            }
        }

        return terrain.terrainData.terrainLayers[maxIndex].name;
    }
}
