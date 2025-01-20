using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 30;
    public int tileCount = 5;
    private List<GameObject> activeTiles = new List<GameObject>();
    
    public Transform playerTransform;
    void Start()
    {
        for(int i=0;i<tileCount;i++)
        {
            if(i==0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0,tilePrefabs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - 35 > zSpawn - (tileCount * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }


    }

    public void SpawnTile(int tileIndex)
    {
        GameObject newTile = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(newTile);
        zSpawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
