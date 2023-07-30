// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SpawnManager : MonoBehaviour
// {
//     public GameObject[] powerupPrefabs;
//     public GameObject[] enemiesPrefabs;
//     private PlayerController playerControllerScript;

//     private Vector3 offset = new Vector3(0, 1, 0);
//     public float spawnRange = 9;
//     public int waveNumber = 1;
//     private int enemyCount;
//     private int bossActive;

//     //Boss Variables: 
//     public GameObject bossPrefab;
//     public int bossRound = 5;
//     public bool isBoss = false;
    
//     // Start is called before the first frame update
//     void Start()
//     {
//         playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
//         SpawnEnemyWave(waveNumber);
//         Debug.Log("Wave: " + waveNumber);

//         //Powerups would spawn every 10 seconds. 
//         InvokeRepeating("SpawnPowerups", 2, 10);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //This function allows you to check how many "Enemy" and Powerup Objects are currently in the scene. 
//         enemyCount = FindObjectsOfType<Enemy>().Length;
//         bossActive = FindObjectsOfType<Boss>().Length;

//         //If there are 0 enemies on the scene, then it will spawn some more enemies. 
//         if(enemyCount == 0 && !playerControllerScript.gameOver) {
//             if(waveNumber % bossRound == 0 && bossActive == 0) {
//                 waveNumber++;
//                 Debug.Log("Wave: " + waveNumber);
//                 isBoss = true;
//                 SpawnBoss();
//             }
//             else if(bossActive == 0){
//                 waveNumber++;
//                 Debug.Log("Wave: " + waveNumber);
//                 isBoss = false;
//                 SpawnEnemyWave(waveNumber);
//             }
//         }

//         //If it is gameover, cancel the invoke calling and destroy all powerups. 
//         if(playerControllerScript.gameOver) {
//             CancelInvoke();
//             GameObject[] powerupsOnScene = GameObject.FindGameObjectsWithTag("Powerup");
//             foreach(GameObject powerup in powerupsOnScene) {
//                 Destroy(powerup);
//             }
//         }
//     }

//     private void SpawnEnemyWave(int enemiesToSpawn) {
//         for(int i=0; i < enemiesToSpawn; i++) {
//             int randomEnemy = Random.Range(0, enemiesPrefabs.Length);
//             Instantiate(enemiesPrefabs[randomEnemy], GenerateSpawnPosition(), enemiesPrefabs[randomEnemy].transform.rotation);
//         }
//     }

//     private Vector3 GenerateSpawnPosition() {
//         float spawnPosX = Random.Range(-spawnRange, spawnRange);
//         float spawnPosZ = Random.Range(-spawnRange, spawnRange);
//         Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
//         return randomPos;
//     }

//     private void SpawnPowerups() {
//         int randPowerup = Random.Range(0, powerupPrefabs.Length);
//         Instantiate(powerupPrefabs[randPowerup], GenerateSpawnPosition() + offset, powerupPrefabs[randPowerup].transform.rotation);
//     }

//     private void SpawnBoss() {
//         Debug.Log("Boss Round!");
//         Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
//     }
//  }
