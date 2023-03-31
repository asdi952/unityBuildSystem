using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainFrom : MonoBehaviour
{
    Terrain terrain;    
    void Start()
    {
        terrain = gameObject.GetComponent<Terrain>();
        if( terrain == null ){
            Debug.LogError("Error: terrain not found");
            return;
        }
        
        changeTerrain( 0);

    }

    float calcDistance( float x, float y){
        return Mathf.Sqrt( Mathf.Pow( x, 2 ) + Mathf.Pow( y,2));
    }
    void changeTerrain( float offset){
        Terrain terrain = GetComponent<Terrain>();

        TerrainData terrainData = terrain.terrainData;

        // Calculate the position of the top-left vertex in the terrain's heightmap array
        int startX = Mathf.RoundToInt((100 / terrainData.size.x) * terrainData.heightmapResolution);
        int startZ = Mathf.RoundToInt((100 / terrainData.size.z) * terrainData.heightmapResolution);

        int centerX = Mathf.RoundToInt((350 / terrainData.size.x) * terrainData.heightmapResolution);
        int centerZ = Mathf.RoundToInt((350 / terrainData.size.z) * terrainData.heightmapResolution);

        // Calculate the size of the rectangular area in the heightmap array
        int width = Mathf.RoundToInt((600 / terrainData.size.x) * terrainData.heightmapResolution);
        int height = Mathf.RoundToInt((600 / terrainData.size.z) * terrainData.heightmapResolution);

        float[,] heights = terrainData.GetHeights(startX, startZ, width, height);
        float[,,] alphamap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapResolution, terrainData.alphamapResolution);
        int xx = 0;
        int yy = 0;
        alphamap[xx, yy, 0] = 0.0f;
        alphamap[xx + 1, yy, 0] = 0.0f;
        alphamap[xx, yy + 1, 0] = 0.0f;
        alphamap[xx + 1, yy + 1, 0] = 0.0f;
        //terrainData.SetAlphamaps(0, 0, alphamap);
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int mapX = startX + x;
                int mapZ = startZ + z;
                float y = 0.05f * ((1 + Mathf.Sin( offset + 2* Mathf.PI * calcDistance(mapX - centerX, mapZ - centerZ)/30))/2);
                heights[z, x] = 0;
            }
        }
        terrainData.SetHeights(startX, startZ, heights);
    }

   
}
