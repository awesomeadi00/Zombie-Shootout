using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    private float zRange = 56;
    private float xRange = 59;
    private PlayerStats playerStats;

    [SerializeField] GameObject zombiePrefab;
    private int waveNumber = 1;
    private int zombieCount;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        SpawnZombieWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        zombieCount = FindObjectsOfType<ZombieController>().Length;

        if(zombieCount == 0) {
            waveNumber++;
            SpawnZombieWave(waveNumber);
        }

        if(playerStats.DeathStatus()) {
            //Destroy every zombie on the map if it is game over. 
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
            foreach(GameObject zombie in zombies) {
                Destroy(zombie);
            } 

            //Add a section here for the boss. 
        }
    }

    //This function simply spawns a certain number of zombies at a random position within the colliders
    private void SpawnZombieWave(int enemiesToSpawn) {
        enemiesToSpawn = Mathf.FloorToInt(enemiesToSpawn * 1.5f) + 7;
        Debug.Log(enemiesToSpawn);
        for(int i=0; i < enemiesToSpawn; i++) {
            Instantiate(zombiePrefab, GenerateSpawnPosition(), zombiePrefab.transform.rotation);
        }
    }


    //This function generates a random spawn position within the colliders and gets the relative y position of the terrain based on the x and z coordinates.
    private Vector3 GenerateSpawnPosition() {
        float spawnPosX = Random.Range(transform.position.x - xRange, transform.position.x + xRange);
        float spawnPosZ = Random.Range(transform.position.z - zRange, transform.position.z + zRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        randomPos.y = Terrain.activeTerrain.SampleHeight(randomPos) + 0.5f;
        Debug.Log("Sample Height: " + Terrain.activeTerrain.SampleHeight(randomPos));
        Debug.Log("Random Pos Y: " + randomPos.y);
        return randomPos;
    }
}
