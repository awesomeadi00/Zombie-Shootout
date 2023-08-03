using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombieSpawnManager : MonoBehaviour
{   
    //Spawn Position Variables: 
    private float zRange = 56;
    private float xRange = 59;
    private float rayOriginHeight = 25;
    private float terrainHeightY;

    private PlayerStats playerStats;
    private ZombieStats zombieStats;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] TextMeshProUGUI roundNumberText;
    [SerializeField] TextMeshProUGUI roundText;

    //Spawn Data Variables: 
    private int[] zombieCountArray_FirstNineteenRounds = {6, 8, 13, 18, 24, 27, 28, 28, 29, 33, 34, 36, 39, 41, 44, 47, 50, 53, 56};
    private float beginningRoundDelay = 10;
    private bool roundStartSection = true;
    private bool roundEndSection = false;
    private int waveNumber = 1;
    private int enemiesToSpawn;
    private int zombieCount;

    //Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        zombieStats = GetComponent<ZombieStats>();
    }

    //Update is called once per frame
    void Update()
    {
        zombieCount = FindObjectsOfType<ZombieController>().Length;

        //There are two moments where there are 0 zombies on the map, at the start of the round and at the end of a round. 
        if(zombieCount == 0 && playerStats.DeathStatus() == false) {
            //If there are 0 zombies at the start, initiate a Coroutine to spawn a zombie wave. 
            if(roundStartSection) {
                StartCoroutine(SpawnZombieWave(waveNumber));
                roundStartSection = false;  //Calls the coroutine once.      
            }

            //Once there are 0 zombies at the end, increment the wave number and deactivate the end-section and activate the start-section.
            //This way, in the next frame Update(), it will initiate a Coroutine to spawn the next zombie wave. 
            if(roundEndSection) {
                waveNumber++;
                roundStartSection = true;
                roundEndSection = false;
            }
        }

        //Once a zombie has spawned, then indicate the roundEndSection as active. 
        if(zombieCount >= 1) {
            roundEndSection = true;
        }

        if(Input.GetMouseButtonDown(0)) {
            //Destroy every zombie on the map if it is game over. 
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
            foreach(GameObject zombie in zombies) {
                zombie.SetActive(false);
            } 
                            //Add a section here for the boss. 
        }
    }

    //This function simply spawns a certain number of zombies at a random position within the colliders
    IEnumerator SpawnZombieWave(int waveNumber) {
        //Displays round number in read before starting the round. 
        roundNumberText.text = waveNumber.ToString();
        roundNumberText.color = Color.red;
        roundText.color = Color.red;

        //Wait for 10 seconds. 
        yield return new WaitForSeconds(beginningRoundDelay);

        //After 10 seconds, display the round number in white to indicate it started. 
        roundNumberText.color = Color.white;
        roundText.color = Color.white;

        //If the round number is less than 20, then spawn the number of zombies from the First Nineteen rounds array
        if(waveNumber < 20) {
            enemiesToSpawn = zombieCountArray_FirstNineteenRounds[waveNumber-1];
            StartCoroutine(SpawnerInstantiation(enemiesToSpawn, waveNumber));
        }

        //Once it hits round 20 and above, then we can officially start spawning enemies through a formula from BO2 zombies spawnrate. 
        else {
            enemiesToSpawn = Mathf.FloorToInt(Mathf.Min(((0.09f * Mathf.Pow(waveNumber, 2)) - (0.0029f * waveNumber) + 23.9850f), 100));
            //If the player manages to reach the higher rounds with a cap of 100, then it will begin to increase the zombies damage to the player 
            if(enemiesToSpawn == 100) {
                zombieStats.damage++;
            }
            StartCoroutine(SpawnerInstantiation(enemiesToSpawn, waveNumber));
        }
    }

    //This function instantiates the zombies depending on the spawn rate. 
    IEnumerator SpawnerInstantiation(int enemies, int round) {
        float spawnRatePerRound = Mathf.Max((4 * (Mathf.Pow(0.95f, (0.5f * round) - 1)) + 1), 2f);

        for(int i=0; i < enemiesToSpawn; i++) {
            yield return new WaitForSeconds(spawnRatePerRound);
            GameObject zombiePrefab = ObjectPooler.SharedInstance.GetPooledObject();
            if (zombiePrefab != null)
            {
                zombiePrefab.SetActive(true);                                   //Activate in hierarchy
                zombiePrefab.transform.position = GenerateSpawnPosition();      //Position it at spawn position
            }
            // Instantiate(zombiePrefab, GenerateSpawnPosition(), zombiePrefab.transform.rotation);
        }
    }

    //This function generates a random spawn position within the colliders and gets the relative y position of the terrain based on the x and z coordinates.
    private Vector3 GenerateSpawnPosition() {
        RaycastHit hit;
        float spawnPosX = Random.Range(transform.position.x - xRange, transform.position.x + xRange);
        float spawnPosZ = Random.Range(transform.position.z - zRange, transform.position.z + zRange);

        //Shoot a ray down from this coordinate (spawn point). 
        Ray ray = new Ray(new Vector3(spawnPosX, rayOriginHeight, spawnPosZ), Vector3.down); 
        
        //If the ray hits a collider, then it will return the y coordinate of that collider which would be the terrain height. 
        if(Physics.Raycast(ray, out hit)) {
            terrainHeightY = hit.point.y;
        }

        //If it doesn't hit a collider, then by default let the terrain height spawn point be 25 as that is the highest point for a zombie to spawn on the terrain.
        else {
            terrainHeightY = rayOriginHeight;
        }
        
        Vector3 randomPos = new Vector3(spawnPosX, terrainHeightY, spawnPosZ);
        return randomPos;
    }
}
