using System.Collections.Generic;
using UnityEngine;

public class ChunkPowerupSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float spawnChance = 0.1f; // 0.1f equals a 10% chance (1 in 10)

    [Header("Powerup Prefabs")]
    public List<GameObject> mediumPowerups;
    public List<GameObject> hardPowerups;

    [Header("Spawn Locations")]
    public List<Transform> powerupSpots;

    [Header("Progression Thresholds")]
    public float hardThreshold = 300f;

    void Start()
    {

        if (Random.value <= spawnChance)
        {
            SpawnPowerup();
        }
    }

    void SpawnPowerup()
    {
        // Safety check to prevent errors if no spots are assigned
        if (powerupSpots.Count == 0) return;

        // Dynamically find the player in the scene to get the score
        PlayerController playerScript = Object.FindAnyObjectByType<PlayerController>();


        float currentScore = playerScript != null ? playerScript.Distance : 0f;

        // Determine which list to pull from based on the score
        List<GameObject> currentActiveList;

        if (currentScore >= hardThreshold && hardPowerups.Count > 0)
        {
            currentActiveList = hardPowerups;
        }
        else if (mediumPowerups.Count > 0)
        {
            currentActiveList = mediumPowerups;
        }
        else
        {
            return; // Failsafe: Do nothing if the lists are empty
        }

        // Pick a random powerup
        int randomItemIndex = Random.Range(0, currentActiveList.Count);
        GameObject powerupToSpawn = currentActiveList[randomItemIndex];

        // Pick a random spot
        int randomSpotIndex = Random.Range(0, powerupSpots.Count);
        Transform spawnLoc = powerupSpots[randomSpotIndex];

        // Spawn the powerup AND parent it to this chunk
        // Passing 'transform' at the end glues the powerup to the chunk, 
        // so if the chunk is destroyed, the uncollected powerup is destroyed too.
        Instantiate(powerupToSpawn, spawnLoc.position, Quaternion.identity, transform);
    }
}